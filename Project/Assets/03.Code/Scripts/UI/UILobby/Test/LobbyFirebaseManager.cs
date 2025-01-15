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

    private RoomData _roomData;
    private LogInUserData _logInUserData;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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

            print($"로그인한 유저의 이름 : {chatUserData.nickname}");
            print($"로그인한 유저의 ID : {chatUserData.id}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase연결 오류 : {e.Message}");
        }
    }


    public async void SendMessage(MessageData messageData)
    {
        try
        {
            _dbChatRef = Database.GetReference(messageData.SenderServer).Child("chats");
            string key = _dbChatRef.Push().Key;

            print($"메시지 보낸사람 Id: {messageData.SenderId}");
            print($"보낸 서버 : {messageData.SenderServer}");
            print($"보낸 내용 : {messageData.Content}");

            string messageJson = JsonConvert.SerializeObject(messageData);
            await _dbChatRef.Child(key).SetRawJsonValueAsync(messageJson);
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase연결 오류: {e.Message}");
        }
    }

    public void ReceiveMessage(Action<MessageData> callback)
    {
        try
        {
            _dbChatRef = Database.GetReference(chatUserData.serverName).Child("chats");

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
                        print($"보낸 내용 : {newMessage.Content}");

                        callback?.Invoke(newMessage);
                    }
                };

            //로그인 한 시간 기준으로 메시지 받음
            _dbChatRef.OrderByChild("TimeStamp")
                .StartAt(logInTime)
                .ChildAdded += _childAddedHandler;
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase 메시지 참조 오류 : {e.Message}");
        }
    }

    public async void CreateRoom(RoomData roomData)
    {
        try
        {
            _dbRoomRef = Database.GetReference(chatUserData.serverName).Child("rooms");
            string roomKey = _dbRoomRef.Push().Key;

            print($"방장의 서버 이름 : {chatUserData.serverName}");
            print($"만든 방의 이름 : {roomData.roomName}");

            RoomData newRoom = new RoomData(
                roomKey,
                roomData.roomName,
                chatUserData.serverName,
                chatUserData.id
                );

            string roomJson = JsonConvert.SerializeObject(newRoom);
            await _dbRoomRef.Child(roomKey).SetRawJsonValueAsync(roomJson);

            print($"방의 Key : {roomKey}");
            MonitorRoomState(newRoom);
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase 방 참조 오류 : {e.Message}");
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
            Debug.LogError($"Firebase 방 참조 오류 : {e}");
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
                Debug.Log("방이 존재하지 않습니다");
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
            print($"Firebase 방 참조 오류 : {e.Message}");
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

                if (string.IsNullOrEmpty(roomData.guest))
                {
                    roomData.guest = chatUserData.id;
                    roomData.state = RoomState.Playing;

                    _roomData = roomData;

                    string updateRoomJson = JsonConvert.SerializeObject(_roomData);
                    await _dbRoomRef.Child(roomData.roomKey).SetRawJsonValueAsync(updateRoomJson);

                    MonitorRoomState(_roomData);
                }
                else
                {
                    Debug.LogError("게스트가 이미 있습니다");
                }
            }
            else
            {
                Debug.LogError("해당 방의 Snapshot이 없음");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase 방 참조 오류 : {e.Message}");
        }
    }

    private RoomState _previousState = RoomState.Waiting;

    private void MonitorRoomState(RoomData roomData)
    {
        _dbRoomRef = Database.GetReference(chatUserData.serverName)
            .Child($"rooms")
            .Child(roomData.roomKey)
            .Child("state");

        _dbRoomRef.ValueChanged += async (sender, args) =>
        {
            if (args.Snapshot.Exists)
            {
                string stateValue = args.Snapshot.Value.ToString();
                print($"바뀌기 전 방 상태 : {stateValue}");

                RoomState newState = (RoomState)Enum.Parse(typeof(RoomState), stateValue);

                if (newState != _previousState)
                {
                    _previousState = newState;

                    print($"바뀐 후 방 상태 : {stateValue}");

                    if (newState == RoomState.Playing)
                    {
                        DatabaseReference lastRoomRef = Database.GetReference(chatUserData.serverName)
                        .Child("rooms")
                        .Child(roomData.roomKey);

                        DataSnapshot snapshot = await lastRoomRef.GetValueAsync();

                        //string json = snapshot.GetRawJsonValue();
                        //RoomData lastRoomData = JsonConvert.DeserializeObject<RoomData>(json);

                        if (snapshot.Exists)
                        {
                            string roomDataJson = snapshot.GetRawJsonValue();
                            PlayerPrefs.SetString("CurrentRoomData", roomDataJson);
                            PlayerPrefs.Save();

                        }
                        SceneManager.LoadScene("OmokScene");
                    }

                    else if (newState == RoomState.Finished)
                    {
                        //여기서는 게임 종료
                    }
                }
            }
        };
    }
}


