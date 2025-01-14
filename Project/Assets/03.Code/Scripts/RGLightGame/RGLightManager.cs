using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

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

		public float limitTime;
		public float startTime;
		public float endTime;
		public bool IsEndGame { get; private set; }
		public bool OverTime { get { return (limitTime <= TimeDiff); } }
		public float RemainTime { get { return limitTime - TimeDiff; } }
		public float TimeDiff { get { return endTime - startTime; } }
		public MainPage MainPage { get; private set; }

		private DatabaseReference _rglightRef;

		private Minigame.RGLight.Player _player;

		private void Awake()
		{
			StartCoroutine(IntroCoroutine());
		}

		private void Start()
		{
			Minigame.RGLight.Player player = Instantiate(_playerPrefab, _startPoint.position, _startPoint.rotation);
			_player = player.GetComponent<Player>();
			player.Init(this);
		}

		private IEnumerator IntroCoroutine()
		{
			_introPanel.SetActive(true);
			yield return new WaitForSeconds(introTime);
			_introPanel.SetActive(false);
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

		public IEnumerator MainPageUpdateCoroutine()
		{
			MainPage = PageManager.Instance.PageOpen<MainPage>();
			while (!IsEndGame)
			{
				MainPage.SetRemainTime(ConvertToMinutesAndSeconds(RemainTime));
				MainPage.SetMoveDistance(_player.playerDistanceTracker.PlayerDistance);
				yield return null;
			}
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
			SetMoney(defaultMoney);
			SetScore(50);

			string durationTime = ConvertToMinutesAndSeconds(TimeDiff);
			PopupManager.Instance.PopupOpen<GameResultPopup>().SetPopup("승리하였소", durationTime, defaultMoney, EndGame);
		}

		private void OnDefeat()
		{
			SetMoney(defaultMoney);
			SetScore(50);

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
					userData.money += value;
				}
				else
				{
					userData = new LogInUserData(GameManager.Instance.FirebaseManager.User.UserId, money: value);
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
				_rglightRef = GameManager.Instance.FirebaseManager.MinigamesRef.Child("rglight");
				DataSnapshot snapshot = await _rglightRef.Child(GameManager.Instance.FirebaseManager.User.UserId).GetValueAsync();
				RGLightUserData userData;

				if (snapshot.Exists)
				{
					userData = JsonConvert.DeserializeObject<RGLightUserData>(snapshot.GetRawJsonValue());
					userData.score += value;
				}
				else
				{
					userData = new RGLightUserData(value);
				}

				string json = JsonConvert.SerializeObject(userData);
				await _rglightRef.Child(GameManager.Instance.FirebaseManager.User.UserId).SetRawJsonValueAsync(json);
			}
			catch (Exception e)
			{
				print(e.Message);
			}
		}
	}
}
