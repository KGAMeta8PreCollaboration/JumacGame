using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class JegiFirebaseManager : Singleton<JegiFirebaseManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    public int _gold;
    public JegiUserData jegiUserData; //여기에서 유저의 최고점수 써야함
    private LogInUserData _logInUserData;

    private DatabaseReference _logInUserDataRef;
    private DatabaseReference _jegiLeaderboardRef;

    protected override void Awake()
    {
        base.Awake();
    }

    private async void Start()
    {
        try
        {
            Auth = GameManager.Instance.FirebaseManager.Auth;
            Database = GameManager.Instance.FirebaseManager.Database;
            User = GameManager.Instance.FirebaseManager.User;

            _logInUserDataRef = Database.GetReference($"loginusers");
            _jegiLeaderboardRef = Database.GetReference($"leaderboard").Child("jegi");

            LogInUserData logInUserData = await SetLogInUserDataByUserId(Auth.CurrentUser.UserId);
            print($"현재 유저 아이디 : {Auth.CurrentUser.UserId}");

            if (logInUserData != null)
            {
                jegiUserData = await SetJegiUserDataByLogInUserData(logInUserData);
                JegiGameManager.Instance.Init();
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"JegiFirebase 초기화 중 오류 발생 {e.Message}");
        }
    }

    private async Task<LogInUserData> SetLogInUserDataByUserId(string id)
    {
        try
        {
            DatabaseReference logInUserDataRef = _logInUserDataRef.Child(id);

            DataSnapshot logInUserDataSnapshot = await logInUserDataRef.GetValueAsync();

            if (logInUserDataSnapshot.Exists)
            {
                string logInUserJson = logInUserDataSnapshot.GetRawJsonValue();
                print($"로그인한 유저의 Json데이터 : {logInUserJson}");
                _logInUserData = JsonConvert.DeserializeObject<LogInUserData>(logInUserJson);
                return _logInUserData;
            }
            else
            {
                Debug.LogWarning("로그인한 유저의 정보가 없습니다!");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"로그인 유저 정보 변환 중 오류 발생 {e.Message}");
            return null;
        }
    }

    private async Task<JegiUserData> SetJegiUserDataByLogInUserData(LogInUserData logInUserData)
    {
        try
        {
            DatabaseReference jegiDataRef = _jegiLeaderboardRef.Child(Auth.CurrentUser.UserId);
            DataSnapshot jegiDataSnapshot = await jegiDataRef.GetValueAsync();
            JegiUserData jegiUserData;

            //존재하면 끌어다씀
            if (jegiDataSnapshot.Exists)
            {
                string jegiUserDataJson = jegiDataSnapshot.GetRawJsonValue();
                jegiUserData = JsonConvert.DeserializeObject<JegiUserData>(jegiUserDataJson);
            }

            else
            {
                jegiUserData = new JegiUserData(logInUserData.nickname, 0);
            }

            _gold = logInUserData.gold;
            return jegiUserData;
        }
        catch (Exception e)
        {
            Debug.LogError($"제기 유저 정보 변환 중 오류발생 {e.Message}");
            return null;
        }
    }

    public async void UpdateJegiUserData(int score, int gold)
    {
        try
        {
            DatabaseReference jegiUserDataRef = _jegiLeaderboardRef.Child(Auth.CurrentUser.UserId);
            DatabaseReference logInUserDataRef = _logInUserDataRef.Child(Auth.CurrentUser.UserId);

            //들어온 점수가 본 점수보다 높으면 
            if (jegiUserData.score <= score)
            {
                //점수 교체
                jegiUserData.score = score;
            }
            _logInUserData.gold += gold;

            string newJegiUserDataJson = JsonConvert.SerializeObject(jegiUserData);
            string newLogInUserDataJson = JsonConvert.SerializeObject(_logInUserData);

            await jegiUserDataRef.SetRawJsonValueAsync(newJegiUserDataJson);
            await logInUserDataRef.SetRawJsonValueAsync(newLogInUserDataJson);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"점수 동기화 중 오류 발생 {e.Message}");
        }
    }
}
