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

public class OmokFirebaseManager : Singleton<OmokFirebaseManager> //���߿� �ı��� �Ǵ� Singleton���� �����ϸ� ���� �� �ϴ�.
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
            //    Debug.LogError($"Firebase ���Ӽ� ���� : {dependencyStatus}");
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

            //    hostData = new OmokUserData(); //���⿡ ���ϴ� ���� �ֱ� �ٵ� LogInUserData�� Gold �ʿ���
            //}
            Test();
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase������ �ȵ� : {e.Message}");
        }
    }

    //id�� ���� OmokUserData�� ����� �Լ�
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
                print($"OmokFirebase���� ȣ�� ���� �α��ε� ������ �̸� : {_longInUserData.nickname}");

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
                Debug.LogError($"���� �����Ͱ� �������� ����");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"���� ������ ���� ���� : {e.Message}");
            return null;
        }
    }

    private void Test()
    {
        print($"ȣ��Ʈ ���� id : {hostData.id}, Name : {hostData.nickname}, gold : {hostData.gold}");
        print($"�Խ�Ʈ ���� id : {guestData.id}, Name : {guestData.nickname}, gold : {guestData.gold}");
    }
}
