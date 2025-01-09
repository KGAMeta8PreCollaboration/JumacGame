using Firebase.Auth;
using Firebase.Database;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LobbyFirebaseManager : Singleton<LobbyFirebaseManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    public ChatUserData chatUserData;

    private DatabaseReference _dbChatRef;
    private DatabaseReference _dbUserRef;
    private DatabaseReference _dbServerRef;

    private LogInUserData _logInUserData;

    private string myTableName = "c";

    private async void Start()
    {
        try
        {
            Auth = FirebaseManager.Instance.Auth;
            Database = FirebaseManager.Instance.Database;
            User = FirebaseManager.Instance.User;

            DatabaseReference logInUserData = Database.GetReference("a").Child("LoginUsers");
            DataSnapshot logInUserSnapshot = await logInUserData.Child(User.UserId).GetValueAsync();

            if (logInUserSnapshot.Exists)
            {
                string logInUserJson = logInUserSnapshot.GetRawJsonValue();
                _logInUserData = JsonConvert.DeserializeObject<LogInUserData>(logInUserJson);

                chatUserData = new ChatUserData
                {
                    nickname = _logInUserData.nickname,
                    id = _logInUserData.id,
                    serverName = _logInUserData.serverName,
                    timestamp = _logInUserData.timestamp
                };
                //������ �� �ʿ��ϸ� ���⿡�� �߰��ϸ� �˴ϴ�.
            }
            else
            {
                Debug.LogWarning("�α��ε� ���� �����͸� ã�� �� �����ϴ�.");
            }

            print($"���� ������ �̸� : {chatUserData.nickname}");
            print($"���� ������ UID : {chatUserData.id}");
        }
        catch (Exception e) 
        {
            Debug.LogError($"Firebase������ �ȵ� : {e.Message}");
        }
    }

    public async void SendMessage(MessageData messageData)
    {
        try
        {
            _dbChatRef = Database.GetReference(myTableName).Child(messageData.SenderServer).Child("chats");
            string key = _dbChatRef.Push().Key;

            print($"���� ���� ��� : {messageData.SenderId}");
            print($"���� : {messageData.SenderServer}");
            print($"���� ���� : {messageData.Content}");

            string messageJson = JsonConvert.SerializeObject(messageData);
            await _dbChatRef.Child(key).SetRawJsonValueAsync(messageJson);
        }
        catch (Exception e)
        {
            Debug.LogError($"�޽��� ���� ����: {e.Message}");
        }
    }
}


