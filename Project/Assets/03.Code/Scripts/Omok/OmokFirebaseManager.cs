using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class OmokFirebaseManager : Singleton<OmokFirebaseManager> //나중에 파괴가 되는 Singleton으로 변경하면 좋을 듯 하다.
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private LogInUserData _logInUserData;
    private DatabaseReference _dbRoomRef;
    private RoomData _currentRoomData;

    public OmokUserData hostData;
    public OmokUserData guestData;

    [SerializeField] private Board board;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        try
        {
            Auth = FirebaseManager.Instance.Auth;
            Database = FirebaseManager.Instance.Database;
            User = FirebaseManager.Instance.User;

            string roomDataJson = PlayerPrefs.GetString("CurrentRoomData", string.Empty);

            if (!string.IsNullOrEmpty(roomDataJson))
            {
                //MonitorRoomState에서 받아온 정보
                _currentRoomData = JsonConvert.DeserializeObject<RoomData>(roomDataJson);
                _dbRoomRef = Database.GetReference(_currentRoomData.serverName)
                    .Child("rooms")
                    .Child(_currentRoomData.roomKey);

                //그 정보를 host와 guest의 OmokUserData로 치환
                hostData = new OmokUserData(_currentRoomData.host);
                guestData = new OmokUserData(_currentRoomData.guest);
                hostData = await SetUserData(hostData);
                guestData = await SetUserData(guestData);

            }
            OmokGameManager.Instance.SetUsers(); //-> 나중에 그냥 UI로 바로 넘겨도 될 듯 함

            MonitorTurnList();
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

    private void MonitorTurnList()
    {
        DatabaseReference turnListRef = _dbRoomRef.Child("turnList");

        turnListRef.ChildAdded += HandleTurnAdded;
    }

    private void HandleTurnAdded(object sender, ChildChangedEventArgs args)
    {
        if (!args.Snapshot.Exists) return;

        try
        {
            string turnJson = args.Snapshot.GetRawJsonValue();
            Turn newTurn = JsonConvert.DeserializeObject<Turn>(turnJson);

            if(newTurn != null)
            {
                Debug.Log($"새턴 : isHostTurn : {newTurn.isHostTurn}, coodinate = {newTurn.coodinate}");

                string[] split = newTurn.coodinate.Split(",");
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);

                board.PlaceStone(newTurn.isHostTurn, new Vector2Int(x, y));

                _currentRoomData.isHostTurn = !newTurn.isHostTurn;
                _currentRoomData.turnCount++;

                UpdateRoomData(_currentRoomData.isHostTurn, _currentRoomData.turnCount);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"턴 테이더 파싱 오류 : {e.Message}");
        }
    }

    private async void UpdateRoomData(bool isHostTurn, int turnCount)
    {
        Dictionary<string, object> updateDic = new Dictionary<string, object>();
        updateDic["isHostTurn"] = isHostTurn;
        updateDic["turnCount"] = turnCount;

        await _dbRoomRef.UpdateChildrenAsync(updateDic);
    }

    public void RequestPlaceStone(Vector2Int boardIndex)
    {
        bool amIHost = User.UserId == _currentRoomData.host;

        if (_currentRoomData.isHostTurn && !amIHost)
        {
            Debug.Log("호스트 턴인데, 나는 게스트! 아직 내 차례가 아님.");
            return;
        }
        if (!_currentRoomData.isHostTurn && amIHost)
        {
            Debug.Log("게스트 턴인데, 나는 호스트! 아직 내 차례가 아님.");
            return;
        }

        Turn newTurn = new Turn
        {
            isHostTurn = amIHost,
            coodinate = $"{boardIndex.x}, {boardIndex.y}"
        };

        AddTurnToFirebase(newTurn);
    }

    private async void AddTurnToFirebase(Turn turn)
    {
        DatabaseReference turnListRef = _dbRoomRef.Child("turnList");

        string turnJson = JsonConvert.SerializeObject(turn);
        string newTurnKey = turnListRef.Push().Key;
        await turnListRef.Child(newTurnKey).SetRawJsonValueAsync(turnJson);

        Debug.Log($"Firebase {turn.coodinate}에 업로드 완료");
    }
}
