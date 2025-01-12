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
        DontDestroyOnLoad(gameObject);
    }

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
            _dbChatRef = Database.GetReference(messageData.SenderServer).Child("chats");
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
            _dbChatRef = Database.GetReference(chatUserData.serverName).Child("chats");

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

    public async void CreateRoom(RoomData roomData)
    {
        try
        {
            _dbRoomRef = Database.GetReference(chatUserData.serverName).Child("rooms");
            string roomKey = _dbRoomRef.Push().Key;

            print($"방 만든사람 서버 : {chatUserData.serverName}");
            print($"만든 방 이름 : {roomData.roomName}");

            RoomData newRoom = new RoomData(
                roomKey,
                roomData.roomName,
                chatUserData.id
                );

            string roomJson = JsonConvert.SerializeObject(newRoom);
            await _dbRoomRef.Child(roomKey).SetRawJsonValueAsync(roomJson);

            print($"방 만들기 완료 : {roomKey}");
            MonitorRoomState(newRoom);
        }
        catch (Exception e)
        {
            Debug.LogError($"방 데이터 만들기 실패 : {e.Message}");
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
            Debug.LogError($"방 삭제 중 오류 발생 : {e}");
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
                Debug.Log("방이 없습니다");
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
            print($"방 찾기 오류 : {e.Message}");
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

                //게스트가 없을 경우
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
            Debug.LogError($"방 참여 중 오류 발생 : {e.Message}");
        }
    }

    private void MonitorRoomState(RoomData roomData)
    {
        _dbRoomRef = Database.GetReference(chatUserData.serverName)
            .Child($"rooms")
            .Child(roomData.roomKey);

        _dbRoomRef.ValueChanged += (sender, args) =>
        {
            if (args.Snapshot.Exists)
            {
                string stateValue = args.Snapshot.Child("state").Value.ToString();
                print($"바뀌기 전 방의 상태 : {stateValue}");

                if (!string.IsNullOrEmpty(stateValue))
                {
                    RoomState newState = (RoomState)Enum.Parse(typeof(RoomState), stateValue);
                    print($"바뀐 방 상태 : {stateValue}");

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


