using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigame.RGLight
{
    public class RGLightManager : MonoBehaviour
    {
        public float introTime;
        public string nextScene;
        [SerializeField] private GameObject _introPanel;
        [SerializeField] private Minigame.RGLight.Player _playerPrefab;
        [SerializeField] private Transform _startPoint;
        public int defaultMoney;



        private void Awake()
        {
            StartCoroutine(IntroCoroutine());
        }

        private void Start()
        {
            Minigame.RGLight.Player player = Instantiate(_playerPrefab, _startPoint.position, _startPoint.rotation);
            player.Init(this);
        }



        private IEnumerator IntroCoroutine()
        {
            _introPanel.SetActive(true);
            yield return new WaitForSeconds(introTime);
            _introPanel.SetActive(false);
        }

        private void EndGame()
        {
            SceneManager.LoadScene(nextScene);
        }

        public void GameResult(bool isSuccess)
        {
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
            SetMoney(defaultMoney);
            PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("성공", "컨트롤이 좋노", EndGame);
        }

        private void OnDefeat()
        {
            SetMoney(defaultMoney);
            PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("실패", "컨트롤이 안좋노", EndGame);
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
                    int money = value;
                    if (int.TryParse(userData.money, out int result)) money += result;
                    userData.money = money.ToString();
                }
                else
                {
                    userData = new LogInUserData(GameManager.Instance.FirebaseManager.User.UserId, money: value.ToString());
                }

                string json = JsonConvert.SerializeObject(userData);
                await logInUsers.Child(GameManager.Instance.FirebaseManager.User.UserId).SetRawJsonValueAsync(json);
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }
    }
}
