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
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class OmokFirebaseManager : Singleton<OmokFirebaseManager> //���߿� �ı��� �Ǵ� Singleton���� �����ϸ� ���� �� �ϴ�.
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
            Auth = GameManager.Instance.FirebaseManager.Auth;
            Database = GameManager.Instance.FirebaseManager.Database;
            User = GameManager.Instance.FirebaseManager.User;

            string roomDataJson = PlayerPrefs.GetString("CurrentRoomData", string.Empty);

            if (!string.IsNullOrEmpty(roomDataJson))
            {
                //MonitorRoomState에서 가져온 CurrentRoomData로 정보 넘겨줌
                _currentRoomData = JsonConvert.DeserializeObject<RoomData>(roomDataJson);
                _dbRoomRef = Database.GetReference(_currentRoomData.serverName)
                    .Child("rooms")
                    .Child(_currentRoomData.roomKey);

                //그 넘겨준 정보로 host와 guest 정보를 OmokUserData로 치환함
                hostData = new OmokUserData(_currentRoomData.host);
                guestData = new OmokUserData(_currentRoomData.guest);
                hostData = await SetUserData(hostData);
                guestData = await SetUserData(guestData);

            }
            OmokGameManager.Instance.SetUsers(); //-> 이건 나중에 OmokUIManager에서 실행해도 좋을 듯 하다

            MonitorTurnList();
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase방 참조 오류 : {e.Message}");
        }
    }

    //id를 OmokUserData로 치환해주는 함수
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
                Debug.LogError($"로그인한 유저의 정보가 없습니다");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase 로그인 유저 참조 오류 : {e.Message}");
            return null;
        }
    }

    private void Test()
    {
        print($"호스트의 정보 id : {hostData.id}, Name : {hostData.nickname}, gold : {hostData.gold}");
        print($"게스트의 정보 id : {guestData.id}, Name : {guestData.nickname}, gold : {guestData.gold}");
    }

    //RoomData에 turnList정보가 바뀔 때 마다 실행됨
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
                Debug.Log($"현재 턴의 정보 : isHostTurn : {newTurn.isHostTurn}, coodinate = {newTurn.coodinate}");

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
            Debug.LogError($"�� ���̴� �Ľ� ���� : {e.Message}");
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
            Debug.Log("현재 호스트 턴인데, 나는 게스트!.");
            return;
        }
        if (!_currentRoomData.isHostTurn && amIHost)
        {
            Debug.Log("현재 게스트 턴인데, 나는 호스트!.");
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
        try
        {
            DatabaseReference turnListRef = _dbRoomRef.Child("turnList");

            string turnJson = JsonConvert.SerializeObject(turn);
            await turnListRef.Child($"{turn.turnCount}").SetRawJsonValueAsync(turnJson);

            Debug.Log($"{turn.turnCount}번째 수 : {turn.coodinate}");
        }
        catch(Exception e)
        {
            Debug.LogError($"Firebase 턴 참조 오류 {e.Message}");
        }
    }
}
