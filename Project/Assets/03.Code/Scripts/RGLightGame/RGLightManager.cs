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

		public bool IsEndGame { get; private set; }
		public float startTime;
		public float endTime;

		private DatabaseReference _rglightRef;

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

		public IEnumerator TimeCheckCoroutine()
		{
			startTime = Time.time;
			while (!IsEndGame)
			{
				endTime = Time.time;
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

		private async void OnSuccess()
		{
			SetMoney(defaultMoney);
			float timeDiff = endTime - startTime;
			string time = timeDiff.ToString("F2");

			if (await NewRecordCheck(timeDiff)) SetDurationTime(time);

			string durationTime = ConvertToMinutesAndSeconds(timeDiff);
			PopupManager.Instance.PopupOpen<GameResultPopup>().SetPopup("승리하였소", durationTime, defaultMoney, EndGame);
		}

		private void OnDefeat()
		{
			SetMoney(defaultMoney);
			string durationTime = ConvertToMinutesAndSeconds(endTime - startTime);
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

		private async void SetDurationTime(string time)
		{
			try
			{
				string json = JsonConvert.SerializeObject(time);
				await _rglightRef.Child(GameManager.Instance.FirebaseManager.User.UserId).SetRawJsonValueAsync(json);
			}
			catch (Exception e)
			{
				print(e.Message);
			}
		}

		public async Task<bool> NewRecordCheck(float time)
		{
			try
			{
				DatabaseReference miniGames = GameManager.Instance.FirebaseManager.MinigamesRef;
				_rglightRef = miniGames.Child("rglight");
				DataSnapshot snapshot = await _rglightRef.Child(GameManager.Instance.FirebaseManager.User.UserId).GetValueAsync();

				if (snapshot.Exists)
				{
					string timeStr = JsonConvert.DeserializeObject<string>(snapshot.GetRawJsonValue());
					if (float.TryParse(timeStr, out float value))
					{
						print(value);
						if (time < value)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
					else
					{
						Debug.LogError("변환할 수 없습니다.");
						return false;
					}
				}
				return true;
			}
			catch (Exception e)
			{
				print(e.Message);
				return false;
			}
		}
	}
}
