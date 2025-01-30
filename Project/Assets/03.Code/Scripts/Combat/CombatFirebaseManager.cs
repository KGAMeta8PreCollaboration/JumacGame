using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CombatFirebaseManager : Singleton<CombatFirebaseManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private LogInUserData _logInUserData;

    private DatabaseReference _dbLogInUserDataRef;

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

            _dbLogInUserDataRef = Database.GetReference($"loginusers");

            _logInUserData = await SetLogInUserDataByUserId(Auth.CurrentUser.UserId);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"CombatFirebase 초기화 중 오류 발생 {e.Message}");
        }
    }

    private async Task<LogInUserData> SetLogInUserDataByUserId(string id)
    {
        try
        {
            DatabaseReference logInUserDataRef = _dbLogInUserDataRef.Child(id);

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

    public async void UpdateUserGold(int gold)
    {
        try
        {
            DatabaseReference logInUserDataRef = _dbLogInUserDataRef.Child(Auth.CurrentUser.UserId);

            _logInUserData.gold += gold;

            string newLogInUserDataJson = JsonConvert.SerializeObject(_logInUserData);
            await logInUserDataRef.SetRawJsonValueAsync(newLogInUserDataJson);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"골드 동기화 중 오류발생 {e.Message}");
        }
    }
}
