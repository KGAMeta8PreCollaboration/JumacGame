using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LogInManager : MonoBehaviour
{
	private DatabaseReference _logInUserRef;

	//회원가입
	public async Task<bool> Create(string email, string password)
	{
		try
		{
			AuthResult result = await FirebaseManager.Instance.Auth.CreateUserWithEmailAndPasswordAsync(email, password);

			LogInUserData userData = new LogInUserData(result.User.UserId);
			string json = JsonConvert.SerializeObject(userData);

			_logInUserRef = FirebaseManager.Instance.Database.GetReference("loginusers");
			await _logInUserRef.Child(result.User.UserId).SetRawJsonValueAsync(json);
			return true;
		}
		catch (Exception e)
		{
			print($"계정 생성 에러 : {e.Message}");

			return false;
		}
	}

	//로그인
	public async Task<bool> SignIn(string email, string password)
	{
		try
		{
			AuthResult result = await FirebaseManager.Instance.Auth.SignInWithEmailAndPasswordAsync(email, password);

			string logInTime = DateTime.UtcNow.ToString("o");

			_logInUserRef = FirebaseManager.Instance.Database.GetReference("loginusers");

			try
			{
				DataSnapshot snapshot = await _logInUserRef.Child(result.User.UserId).GetValueAsync();
				LogInUserData userData;
				if (snapshot.Exists)
				{
					userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
					userData.timestamp = logInTime;
				}
				else
				{
					userData = new LogInUserData(result.User.UserId, logInTime);
				}

				string json = JsonConvert.SerializeObject(userData);
				await _logInUserRef.Child(result.User.UserId).SetRawJsonValueAsync(json);

				Debug.Log($"로그인 성공! 로그인 시간 업데이트: {logInTime}");
				return true;
			}
			catch (Exception e)
			{
				print($"데이터를 가져올 수 없음 : {e.Message}");

				//로그인은 성공했는데 데이터를 못가져 왔으니 로그아웃으로 로그인 못하게 해야 함.
				SignOut();
				return false;
			}
		}
		catch (Exception e)
		{
			print($"로그인 에러 : {e.Message}");

			return false;
		}
	}

	//로그아웃
	public void SignOut()
	{
		FirebaseManager.Instance.Auth.SignOut();
	}

	public async Task<bool> SetNicknameAndRace(string nickname, string race)
	{
		try
		{
			DataSnapshot snapshot = await _logInUserRef.Child($"{FirebaseManager.Instance.User.UserId}").GetValueAsync();
			LogInUserData userData;

			if (snapshot.Exists)
			{
				userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
				userData.nickname = nickname;
				userData.race = race;
				userData.setNicknameRace = true;
			}
			else
			{
				userData = new LogInUserData(FirebaseManager.Instance.User.UserId, nickname: nickname, race: race, setNicknameRace: true);
			}

			string json = JsonConvert.SerializeObject(userData);
			await _logInUserRef.Child($"{FirebaseManager.Instance.User.UserId}").SetRawJsonValueAsync(json);
			return true;
		}
		catch (Exception e)
		{
			print(e.Message);
			return false;
		}
	}

	public async Task<bool> SetServerName(string name)
	{
		try
		{
			DataSnapshot snapshot = await _logInUserRef.Child($"{FirebaseManager.Instance.User.UserId}").GetValueAsync();
			LogInUserData userData;

			if (snapshot.Exists)
			{
				userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
				userData.serverName = name;
			}
			else
			{
				userData = new LogInUserData(FirebaseManager.Instance.User.UserId, serverName: name);
			}

			string json = JsonConvert.SerializeObject(userData);
			await _logInUserRef.Child($"{FirebaseManager.Instance.User.UserId}").SetRawJsonValueAsync(json);
			Debug.Log($"선택한 서버! : {name}");
			return true;
		}
		catch (Exception e)
		{
			print($"서버를 선택할 수 없습니다. : {e.Message}");
			return false;
		}
	}

	public async Task<bool> DuplicateNicknameCheck(string nickname)
	{
		try
		{
			DataSnapshot snapshot = await _logInUserRef.GetValueAsync();

			if (snapshot.Exists)
			{
				foreach (DataSnapshot userSnapshot in snapshot.Children)
				{
					if (userSnapshot.HasChild("nickname"))
					{
						string userNickname = userSnapshot.Child("nickname").Value?.ToString();
						if (userNickname.Equals(nickname))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		catch (Exception e)
		{
			print($"{e.Message}");
			return true;
		}
	}

	public async Task<bool> ExistNicknameAndRace()
	{
		try
		{
			DataSnapshot snapshot = await _logInUserRef.Child(FirebaseManager.Instance.User.UserId).GetValueAsync();
			if (snapshot.Exists)
			{
				if (snapshot.HasChild("setNicknameRace"))
				{
					bool value = (bool)snapshot.Child("setNicknameRace").Value;
					return value;
				}
			}
			return false;
		}
		catch (Exception e)
		{
			print(e.Message);
			return false;
		}
	}
}
