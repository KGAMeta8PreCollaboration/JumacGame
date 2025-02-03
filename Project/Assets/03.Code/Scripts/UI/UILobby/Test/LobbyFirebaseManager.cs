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
    public LogInUserData logInUserData;

    //각각의 Firebase 참조 Reference
    private DatabaseReference _dbChatRef;
    private DatabaseReference _dbUserRef;
    private DatabaseReference _dbServerRef;
    private DatabaseReference _dbRoomRef;

    //채팅 변화를 감지하는 EventHandler
    private EventHandler<ChildChangedEventArgs> _childAddedHandler;

    private RoomData _roomData;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject); //-> 없어도 roomState변경을 추적한다.
    }

    //lobby입장 시 logInUserData, chatUserData, _dbRoomRef를 초기화해줌
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
                this.logInUserData = JsonConvert.DeserializeObject<LogInUserData>(logInUserJson);

                chatUserData = new ChatUserData
                {
                    nickname = this.logInUserData.nickname,
                    id = this.logInUserData.id,
                    servername = this.logInUserData.serverName,
                    timestamp = this.logInUserData.timestamp
                };

                _dbRoomRef = Database.GetReference("omokuserdata")
                .Child("rooms")
                .Child(chatUserData.servername);

                if (_dbRoomRef == null)
                {
                    Debug.LogWarning("룸 참조 실패");
                }
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
            _dbChatRef = Database.GetReference("chats").Child(chatUserData.servername);
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

    public void ReceiveMessage(Action<MessageData> callback = null)
    {
        print("일단 연결은 됨");
        try
        {
            _dbChatRef = Database.GetReference("chats").Child(chatUserData.servername);

            //이미 이벤트가 있으면 전에 있던 이벤트를 해제
            if (_childAddedHandler != null)
            {
                _dbChatRef.ChildAdded -= _childAddedHandler;
            }

            string logInTime = chatUserData.timestamp;

            _childAddedHandler = (sender, args) =>
                {
                    print("메시지의 변화도 감지함");
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
            print("시간으로도 감지함");
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
            string roomKey = _dbRoomRef.Push().Key;

            print($"방장의 서버 이름 : {chatUserData.servername}");
            print($"만든 방의 이름 : {roomData.roomName}");

            //방 생성 당시에는 방이름, 배팅금, 서버이름, 만든이의id만 저장
            RoomData newRoom = new RoomData(
                roomKey,
                roomData.roomName,
                roomData.betting,
                chatUserData.servername,
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
            DatabaseReference deleteRoomData = _dbRoomRef.Child(roomData.roomKey);

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
            DataSnapshot roomSnapshot = await _dbRoomRef.Child(roomData.roomKey).GetValueAsync();

            if (roomSnapshot.Exists)
            {
                string roomDataJson = roomSnapshot.GetRawJsonValue();
                roomData = JsonConvert.DeserializeObject<RoomData>(roomDataJson);
                
                //방 상태가 Waiting일때만 입장 가능
                if (roomData.state != RoomState.Waiting)
                {
                    Debug.LogWarning("해당 방은 이미 게임 중이거나 종료된 상태입니다.");
                    return;
                }

                //게스트가 이미 들어오면 불가능
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
        DatabaseReference roomStateRef = _dbRoomRef.Child(roomData.roomKey).Child("state");

        roomStateRef.ValueChanged += async (sender, args) =>
        {
            if (args.Snapshot.Exists)
            {
                string stateValue = args.Snapshot.Value.ToString();
                print($"바뀐 후 방 상태 : {stateValue}");

                RoomState newState = (RoomState)Enum.Parse(typeof(RoomState), stateValue);

                if (newState != _previousState)
                {
                    _previousState = newState;

                    if (newState == RoomState.Playing)
                    {
                        print("일단 Playing으로 상태 바뀜");
                        SceneManager.LoadScene("OmokScene");
                    }

                    else if (newState == RoomState.Finished)
                    {
                        //게임 끝나고 나서 기록 지우려면 아래 주석 제거
                        //DeleteRoom(roomData);
                    }
                }
            }
        };
    }
}


