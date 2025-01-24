using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Minigame.RGLight
{
    public class RGLightManager : MonoBehaviour
    {
        public float introTime;
        public string nextScene;
        [SerializeField] private GoldSpawner moneySpawner;
        [SerializeField] private Younghee younghee;
        [SerializeField] private Transform _startPoint;

        [Header("Prefabs")]
        [SerializeField] private Minigame.RGLight.Player _playerPrefab;
        [SerializeField] private CageManager _cageManagerPrefab;
        [SerializeField] private RGLightGame _rglightGamePrefab;
        [SerializeField] private YoungheeAnimationUI _youngheeAnimationUIPrefab;

        public int defaultMoney;
        public float limitTime;
        public float startTime;
        public float endTime;

        public float mainPageUpdateInterval;

        public bool IsEndGame { get; private set; }
        public bool OverTime { get { return (limitTime <= TimeDiff); } }
        public float RemainTime { get { return limitTime - TimeDiff; } }
        public float TimeDiff { get { return endTime - startTime; } }
        public MainPage MainPage { get; private set; }

        private DatabaseReference _rglightRef;

        public Minigame.RGLight.Player player { get; private set; }
        public CageManager CageManager { get; private set; }
        private RGLightGame _rglightGame;
        private YoungheeAnimationUI _youngheeAnimationUI;
        private int _addMoney;

        private void Awake()
        {
            player = Instantiate(_playerPrefab, _startPoint.position, _startPoint.rotation);
            player.Init(this);
        }

        private void Start()
        {
            CageManager = Instantiate(_cageManagerPrefab, transform);
            _rglightGame = Instantiate(_rglightGamePrefab, transform);
            _youngheeAnimationUI = Instantiate(_youngheeAnimationUIPrefab, GameObject.Find("Canvas").transform);

            CageManager.Init(this);
            _rglightGame.Init(this);
            _youngheeAnimationUI.Init(this);

            _rglightGame.endSentenceAction = OnEndSentence;
            younghee.endSkillAction = OnEndSkill;
        }

        public void OnEndSentence()
        {
            if (IsEndGame) return;
            StartCoroutine(_youngheeAnimationUI.YoungheeAnimation(true));
            CageManager.Spawn(player.PlayerRay.CalcSpawnPoint());
            moneySpawner.Spawn(player.PlayerRay.CalcSpawnPoint(), player.PlayerDistanceTracker.GetMoney());
            younghee.UseSkill();
        }

        public void OnEndSkill()
        {
            if (IsEndGame) return;
            StartCoroutine(_youngheeAnimationUI.YoungheeAnimation(false));
            CageManager.DestroyCage();
            StartCoroutine(_rglightGame.ReadSentence2());
        }

        public IEnumerator TimeCheckCoroutine()
        {
            startTime = Time.time;
            while (!IsEndGame)
            {
                endTime = Time.time;
                if (OverTime) GameResult(false);
                yield return null;
            }
        }

        public void GameStart()
        {
            StartCoroutine(_rglightGame.ReadSentence2());

            _youngheeAnimationUI.ShowImage(true);
        }

        public IEnumerator MainPageUpdateCoroutine()
        {
            MainPage = PageManager.Instance.PageOpen<MainPage>();
            while (!IsEndGame)
            {
                MainPage.SetRemainTime(ConvertToMinutesAndSeconds(RemainTime));
                MainPage.SetMoveDistance(player.PlayerDistanceTracker.PlayerDistance);
                yield return new WaitForSeconds(mainPageUpdateInterval);
            }
        }

        public void AddMoney(int value)
        {
            _addMoney += value;
        }

        private void EndGame()
        {
            SceneManager.LoadScene(nextScene);
        }

        public void GameResult(bool isSuccess)
        {
            IsEndGame = true;
            if (isSuccess)
            {
                OnSuccess();
            }
            else
            {
                OnDefeat();
            }
        }

        private void OnSuccess()
        {
            SetMoney(defaultMoney + _addMoney);
            SetScore(player.PlayerDistanceTracker.GetScore());

            string durationTime = ConvertToMinutesAndSeconds(TimeDiff);
            PopupManager.Instance.PopupOpen<GameResultPopup>().SetPopup("승리하였소", durationTime, defaultMoney, EndGame);
        }

        private void OnDefeat()
        {
            SetMoney(_addMoney);
            SetScore(player.PlayerDistanceTracker.GetScore());

            string durationTime = ConvertToMinutesAndSeconds(TimeDiff);
            PopupManager.Instance.PopupOpen<GameResultPopup>().SetPopup("형편 없이 졌소", durationTime, defaultMoney, EndGame);
        }

        string ConvertToMinutesAndSeconds(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time % 1) * 100);
            return $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
        }

        private async void SetMoney(int value)
        {
            try
            {
                DatabaseReference logInUsers = GameManager.Instance.FirebaseManager.LogInUsersRef;
                DataSnapshot snapshot = await logInUsers.Child(GameManager.Instance.FirebaseManager.User.UserId).GetValueAsync();
                LogInUserData userData;

                if (snapshot.Exists)
                {
                    userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
                    userData.gold += value;
                }
                else
                {
                    userData = new LogInUserData(GameManager.Instance.FirebaseManager.User.UserId, gold: value);
                }

                string json = JsonConvert.SerializeObject(userData);
                await logInUsers.Child(GameManager.Instance.FirebaseManager.User.UserId).SetRawJsonValueAsync(json);
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }

        private async void SetScore(int value)
        {
            try
            {
                _rglightRef = GameManager.Instance.FirebaseManager.LeaderBoardRef.Child("rglight");
                DataSnapshot snapshot = await _rglightRef.Child(GameManager.Instance.FirebaseManager.User.UserId).GetValueAsync();
                RGLightUserData userData;

                string nickname = await GetNickname();

                if (snapshot.Exists)
                {
                    userData = JsonConvert.DeserializeObject<RGLightUserData>(snapshot.GetRawJsonValue());
                    userData.nickname = nickname;
                    userData.score += value;
                }
                else
                {
                    userData = new RGLightUserData(nickname, value);
                }

                string json = JsonConvert.SerializeObject(userData);
                await _rglightRef.Child(GameManager.Instance.FirebaseManager.User.UserId).SetRawJsonValueAsync(json);
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }

        public async Task<string> GetNickname()
        {
            try
            {
                DataSnapshot snapshot = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child(GameManager.Instance.FirebaseManager.User.UserId).GetValueAsync();
                if (snapshot.Exists)
                {
                    if (snapshot.HasChild("nickname"))
                    {
                        string nickname = snapshot.Child("nickname").Value.ToString();
                        return nickname;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception e)
            {
                print(e.Message);
                return null;
            }
        }

        public async Task<string> GetRace()
        {
            try
            {
                DataSnapshot snapshot = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child(GameManager.Instance.FirebaseManager.User.UserId).GetValueAsync();
                if (snapshot.Exists)
                {
                    if (snapshot.HasChild("race"))
                    {
                        string race = snapshot.Child("race").Value.ToString();
                        return race;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception e)
            {
                print(e.Message);
                return null;
            }
        }

        private void OnDestroy()
        {
            _rglightGame.endSentenceAction -= OnEndSentence;
            younghee.endSkillAction -= OnEndSkill;
        }
    }
}
