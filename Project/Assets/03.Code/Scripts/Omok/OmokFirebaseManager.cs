using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OmokFirebaseManager : Singleton<OmokFirebaseManager> //나중에 파괴가 되는 Singleton으로 변경하면 좋을 듯 하다.
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private LogInUserData _logInUserData;
    public OmokUserData omokUserData;

    private async void Start()
    {
        try
        {
            Auth = FirebaseManager.Instance.Auth;
            Database = FirebaseManager.Instance.Database;
            User = FirebaseManager.Instance.User;

            DatabaseReference logInUserData = Database.GetReference("loginusers");
            DataSnapshot logInUserSnapshot = await logInUserData.Child(User.UserId).GetValueAsync();

            if (logInUserSnapshot.Exists)
            {
                string logInUserJson = logInUserSnapshot.GetRawJsonValue();
                _logInUserData = JsonConvert.DeserializeObject<LogInUserData>(logInUserJson);

                omokUserData = new OmokUserData(); //여기에 원하는 정보 넣기
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase연결이 안됨 : {e.Message}");
        }
    }
}
