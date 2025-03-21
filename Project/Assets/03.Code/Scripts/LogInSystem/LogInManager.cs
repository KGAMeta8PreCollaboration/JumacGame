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
    private DatabaseReference _logInUsersRef;

    public async Task<bool> Create(string email, string password)
    {
        try
        {
            AuthResult result = await GameManager.Instance.FirebaseManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password);

            LogInUserData userData = new LogInUserData(result.User.UserId);
            string json = JsonConvert.SerializeObject(userData);

            _logInUsersRef = GameManager.Instance.FirebaseManager.LogInUsersRef;
            await _logInUsersRef.Child(result.User.UserId).SetRawJsonValueAsync(json);
            return true;
        }
        catch (Exception e)
        {
            print($"���� ���� ���� : {e.Message}");

            return false;
        }
    }

    public async Task<bool> SignIn(string email, string password)
    {
        try
        {
            AuthResult result = await GameManager.Instance.FirebaseManager.Auth.SignInWithEmailAndPasswordAsync(email, password);

            string logInTime = DateTime.UtcNow.ToString("o");

            _logInUsersRef = GameManager.Instance.FirebaseManager.LogInUsersRef;

            try
            {
                DataSnapshot snapshot = await _logInUsersRef.Child(result.User.UserId).GetValueAsync();
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
                await _logInUsersRef.Child(result.User.UserId).SetRawJsonValueAsync(json);

                Debug.Log($"�α��� ����! �α��� �ð� ������Ʈ: {logInTime}");
                return true;
            }
            catch (Exception e)
            {
                print($"�����͸� ������ �� ���� : {e.Message}");
                Debug.LogWarning($"1111 :{e.Data}, {e.Message}");
                SignOut();
                return false;
            }
        }
        catch (Exception e)
        {
            print($"�α��� ���� : {e.Message}");
            Debug.LogWarning($"2222 : {e.Data},  {e.Message}");

            return false;
        }
    }

    public void SignOut()
    {
        GameManager.Instance.FirebaseManager.Auth.SignOut();
    }

    public async Task<bool> SetNicknameAndRace(string nickname, string race)
    {
        try
        {
            DataSnapshot snapshot = await _logInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}").GetValueAsync();
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
                userData = new LogInUserData(GameManager.Instance.FirebaseManager.User.UserId, nickname: nickname, race: race, setNicknameRace: true);
            }

            string json = JsonConvert.SerializeObject(userData);
            await _logInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}").SetRawJsonValueAsync(json);
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
            DataSnapshot snapshot = await _logInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}").GetValueAsync();
            LogInUserData userData;

            if (snapshot.Exists)
            {
                userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
                userData.serverName = name;
            }
            else
            {
                userData = new LogInUserData(GameManager.Instance.FirebaseManager.User.UserId, serverName: name);
            }

            string json = JsonConvert.SerializeObject(userData);
            await _logInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}").SetRawJsonValueAsync(json);
            Debug.Log($"������ ����! : {name}");
            return true;
        }
        catch (Exception e)
        {
            print($"������ ������ �� �����ϴ�. : {e.Message}");
            return false;
        }
    }

    public async Task<bool> DuplicateNicknameCheck(string nickname)
    {
        try
        {
            DataSnapshot snapshot = await _logInUsersRef.GetValueAsync();

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
            DataSnapshot snapshot = await _logInUsersRef.Child(GameManager.Instance.FirebaseManager.User.UserId).GetValueAsync();
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

    public async Task<string> ServerPopulation(string serverName)
    {
        string @return = "0 / 100";
        try
        {

            DataSnapshot snapshot = await GameManager.Instance.FirebaseManager.LobbyUsersRef.Child(serverName).GetValueAsync();
            if (snapshot.Exists)
            {
                if (snapshot.HasChild("userlist"))
                {
                    long curPopulation = snapshot.Child("userlist").ChildrenCount;
                    @return = $"{curPopulation} / 100";
                    return @return;
                }
            }
            return @return;
        }
        catch (Exception e)
        {
            print(e.Message);
            return @return;
        }
    }
}
