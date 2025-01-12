using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class OmokFirebaseManager : Singleton<OmokFirebaseManager> //나중에 파괴가 되는 Singleton으로 변경하면 좋을 듯 하다.
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private LogInUserData _logInUserData;
    public OmokUserData hostData;
    public OmokUserData guestData;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        try
        {
            //var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            //if (dependencyStatus != DependencyStatus.Available)
            //{
            //    Debug.LogError($"Firebase 종속성 오류 : {dependencyStatus}");
            //    return;
            //}
            Auth = FirebaseManager.Instance.Auth;
            Database = FirebaseManager.Instance.Database;
            User = FirebaseManager.Instance.User;

            string roomDataJson = PlayerPrefs.GetString("CurrentRoomData", string.Empty);

            if (!string.IsNullOrEmpty(roomDataJson))
            {
                RoomData roomData = JsonConvert.DeserializeObject<RoomData>(roomDataJson);
                hostData = new OmokUserData(roomData.host);
                guestData = new OmokUserData(roomData.guest);

                hostData = await SetUserData(hostData);
                guestData = await SetUserData(guestData);
            }

            //DatabaseReference logInUserData = Database.GetReference("loginusers");
            //DataSnapshot logInUserSnapshot = await logInUserData.Child(User.UserId).GetValueAsync();

            //if (logInUserSnapshot.Exists)
            //{
            //    string logInUserJson = logInUserSnapshot.GetRawJsonValue();
            //    _logInUserData = JsonConvert.DeserializeObject<LogInUserData>(logInUserJson);

            //    hostData = new OmokUserData(); //여기에 원하는 정보 넣기 근데 LogInUserData에 Gold 필요함
            //}
            Test();
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase연결이 안됨 : {e.Message}");
        }
    }

    //id를 통해 OmokUserData를 만드는 함수
    private async Task<OmokUserData> SetUserData(OmokUserData userData)
    {
        try
        {
            DatabaseReference logInUserData = Database.GetReference("loginusers");
            DataSnapshot logInUserSnapshot = await logInUserData.Child(userData.id).GetValueAsync();

            if (logInUserSnapshot.Exists)
            {
                string logInUserJson = logInUserSnapshot.GetRawJsonValue();
                LogInUserData _longInUserData = JsonConvert.DeserializeObject<LogInUserData>(logInUserJson);
                print($"OmokFirebase에서 호출 현재 로그인된 유저의 이름 : {_longInUserData.nickname}");

                OmokUserData _userData = new OmokUserData
                (
                    _longInUserData.id,
                    _longInUserData.nickname,
                    0
                );

                return _userData;
            }
            else
            {
                Debug.LogError($"유저 데이터가 존재하지 않음");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"유저 데이터 세팅 실패 : {e.Message}");
            return null;
        }
    }

    private void Test()
    {
        print($"호스트 정보 id : {hostData.id}, Name : {hostData.nickname}, gold : {hostData.gold}");
        print($"게스트 정보 id : {guestData.id}, Name : {guestData.nickname}, gold : {guestData.gold}");
    }
}
