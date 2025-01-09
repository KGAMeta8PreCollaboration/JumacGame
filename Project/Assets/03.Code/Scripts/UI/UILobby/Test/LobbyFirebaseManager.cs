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

    private EventHandler<ChildChangedEventArgs> _childAddedHandler;

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
                //정보가 더 필요하면 여기에서 추가하면 됩니다.
            }
            else
            {
                Debug.LogWarning("로그인된 유저 데이터를 찾을 수 없습니다.");
            }

            print($"현재 유저의 이름 : {chatUserData.nickname}");
            print($"현재 유저의 UID : {chatUserData.id}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase연결이 안됨 : {e.Message}");
        }
    }

    public async void SendMessage(MessageData messageData)
    {
        try
        {
            _dbChatRef = Database.GetReference(myTableName).Child(messageData.SenderServer).Child("chats");
            string key = _dbChatRef.Push().Key;

            print($"현재 보낸 사람 : {messageData.SenderId}");
            print($"서버 : {messageData.SenderServer}");
            print($"보낸 내용 : {messageData.Content}");

            string messageJson = JsonConvert.SerializeObject(messageData);
            await _dbChatRef.Child(key).SetRawJsonValueAsync(messageJson);
        }
        catch (Exception e)
        {
            Debug.LogError($"메시지 전송 실패: {e.Message}");
        }
    }

    public void ReceiveMessage(Action<MessageData> callback)
    {
        try
        {
            _dbChatRef = Database.GetReference(myTableName).Child(chatUserData.serverName).Child("chats");

            //기존에 남아있을 수 있는 이벤트 제거
            if (_childAddedHandler != null)
            {
                _dbChatRef.ChildAdded -= _childAddedHandler;
            }

            string logInTime = chatUserData.timestamp;

            _childAddedHandler = (sender, args) =>
                {
                    if (args.Snapshot.Exists)
                    {
                        string json = args.Snapshot.GetRawJsonValue();
                        MessageData newMessage = JsonConvert.DeserializeObject<MessageData>(json);

                        print($"메시지 보낸 사람 : {newMessage.SenderId}");
                        print($"메시지 내용 : {newMessage.Content}");

                        callback?.Invoke(newMessage);
                    }
                };

            _dbChatRef.OrderByChild("TimeStamp")
                .StartAt(logInTime)
                .ChildAdded += _childAddedHandler;
        }
        catch (Exception e)
        {
            Debug.LogError($"메시지 수신 오류: {e.Message}");
        }

    }
}


