using Firebase.Auth;
using Firebase.Database;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LobbyFirebaseManager : Singleton<LobbyFirebaseManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    public ChatUserData chatUserData;

    private DatabaseReference _dbChatRef;
    private DatabaseReference _dbUserRef;
    private DatabaseReference _dbServerRef;
    private DatabaseReference _dbRoomRef;

    private EventHandler<ChildChangedEventArgs> _childAddedHandler;

    private LogInUserData _logInUserData;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        try
        {
            Auth = GameManager.Instance.FirebaseManager.Auth;
            Database = GameManager.Instance.FirebaseManager.Database;
            User = GameManager.Instance.FirebaseManager.User;

            DatabaseReference logInUserData = Database.GetReference("loginusers");
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
            }
            else
            {
                Debug.LogError("로그인 할 유저의 Id가 없습니다.");
            }

            print($" : {chatUserData.nickname}");
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
            _dbChatRef = Database.GetReference(messageData.SenderServer).Child("chats");
            string key = _dbChatRef.Push().Key;

            print($"���� ���� ���?: {messageData.SenderId}");
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

    public void ReceiveMessage(Action<MessageData> callback)
    {
        try
        {
            _dbChatRef = Database.GetReference(chatUserData.serverName).Child("chats");

            //������ �������� �� �ִ� �̺�Ʈ ����
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

                        print($"�޽��� ���� ���?: {newMessage.SenderId}");
                        print($"�޽��� ���� : {newMessage.Content}");

                        callback?.Invoke(newMessage);
                    }
                };

            _dbChatRef.OrderByChild("TimeStamp")
                .StartAt(logInTime)
                .ChildAdded += _childAddedHandler;
        }
        catch (Exception e)
        {
            Debug.LogError($"�޽��� ���� ����: {e.Message}");
        }
    }

    public async void CreateRoom(RoomData roomData)
    {
        try
        {
            _dbRoomRef = Database.GetReference(chatUserData.serverName).Child("rooms");
            string roomKey = _dbRoomRef.Push().Key;

            print($"�� ������ ���� : {chatUserData.serverName}");
            print($"���� �� �̸� : {roomData.roomName}");

            RoomData newRoom = new RoomData(
                roomKey,
                roomData.roomName,
                chatUserData.serverName,
                chatUserData.id
                );

            string roomJson = JsonConvert.SerializeObject(newRoom);
            await _dbRoomRef.Child(roomKey).SetRawJsonValueAsync(roomJson);

            print($"�� ����� �Ϸ� : {roomKey}");
            MonitorRoomState(newRoom);
        }
        catch (Exception e)
        {
            Debug.LogError($"�� ������ �����?���� : {e.Message}");
        }
    }

    public async void DeleteRoom(RoomData roomData)
    {
        try
        {
            _dbRoomRef = Database.GetReference(chatUserData.serverName)
                .Child("rooms")
                .Child(roomData.roomKey);
            await _dbRoomRef.RemoveValueAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"�� ���� �� ���� �߻� : {e}");
        }
    }

    public async Task<List<RoomData>> FindRoom()
    {
        try
        {
            _dbRoomRef = Database.GetReference(chatUserData.serverName).Child("rooms");
            DataSnapshot roomSnapshot = await _dbRoomRef.GetValueAsync();

            List<RoomData> waitingRoomList = new List<RoomData>();

            if (!roomSnapshot.Exists)
            {
                Debug.Log("���� �����ϴ�");
                return waitingRoomList;
            }

            foreach (DataSnapshot snapshot in roomSnapshot.Children)
            {
                string roomJson = snapshot.GetRawJsonValue();
                RoomData roomData = JsonConvert.DeserializeObject<RoomData>(roomJson);

                if (roomData.state == RoomState.Waiting)
                {
                    waitingRoomList.Add(roomData);
                }
            }

            return waitingRoomList;
        }
        catch (Exception e)
        {
            print($"�� ã�� ���� : {e.Message}");
            return null;
        }
    }

    public async void JoinRoom(RoomData roomData, ChatUserData chatUserData)
    {
        try
        {
            _dbRoomRef = Database.GetReference(chatUserData.serverName).Child($"rooms");
            DataSnapshot roomSnapshot = await _dbRoomRef.Child(roomData.roomKey).GetValueAsync();

            if (roomSnapshot.Exists)
            {
                string roomDataJson = roomSnapshot.GetRawJsonValue();
                roomData = JsonConvert.DeserializeObject<RoomData>(roomDataJson);

                //�Խ�Ʈ�� ���� ���
                if (string.IsNullOrEmpty(roomData.guest))
                {
                    roomData.guest = chatUserData.id;
                    roomData.state = RoomState.Playing;

                    string updateRoomJson = JsonConvert.SerializeObject(roomData);
                    await _dbRoomRef.Child(roomData.roomKey).SetRawJsonValueAsync(updateRoomJson);

                    MonitorRoomState(roomData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"�� ���� �� ���� �߻� : {e.Message}");
        }
    }

    private void MonitorRoomState(RoomData roomData)
    {
        _dbRoomRef = Database.GetReference(chatUserData.serverName)
            .Child($"rooms")
            .Child(roomData.roomKey)
            .Child("state");

        _dbRoomRef.ValueChanged += (sender, args) =>
        {
            if (args.Snapshot.Exists)
            {
                string stateValue = args.Snapshot.Value.ToString();
                print($"�ٲ�� �� ���� ���� : {stateValue}");

                //�� ���°� �ٲ𶧸� ����
                if (!string.IsNullOrEmpty(stateValue))
                {
                    RoomState newState = (RoomState)Enum.Parse(typeof(RoomState), stateValue);
                    print($"�ٲ� �� ���� : {stateValue}");

                    if (newState == RoomState.Playing)
                    {
                        string roomDataJson = JsonConvert.SerializeObject(roomData);
                        PlayerPrefs.SetString("CurrentRoomData", roomDataJson);
                        PlayerPrefs.Save();

                        SceneManager.LoadScene("OmokScene");
                    }
                }
            }
        };
    }
}


