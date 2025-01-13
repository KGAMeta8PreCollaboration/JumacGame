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
                //MonitorRoomState���� �޾ƿ� ����
                _currentRoomData = JsonConvert.DeserializeObject<RoomData>(roomDataJson);
                _dbRoomRef = Database.GetReference(_currentRoomData.serverName)
                    .Child("rooms")
                    .Child(_currentRoomData.roomKey);

                //�� ������ host�� guest�� OmokUserData�� ġȯ
                hostData = new OmokUserData(_currentRoomData.host);
                guestData = new OmokUserData(_currentRoomData.guest);
                hostData = await SetUserData(hostData);
                guestData = await SetUserData(guestData);

            }
            OmokGameManager.Instance.SetUsers(); //-> ���߿� �׳� UI�� �ٷ� �Ѱܵ� �� �� ��

            MonitorTurnList();
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
                Debug.Log($"���� : isHostTurn : {newTurn.isHostTurn}, coodinate = {newTurn.coodinate}");

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
            Debug.Log("ȣ��Ʈ ���ε�, ���� �Խ�Ʈ! ���� �� ���ʰ� �ƴ�.");
            return;
        }
        if (!_currentRoomData.isHostTurn && amIHost)
        {
            Debug.Log("�Խ�Ʈ ���ε�, ���� ȣ��Ʈ! ���� �� ���ʰ� �ƴ�.");
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

        Debug.Log($"Firebase {turn.coodinate}�� ���ε� �Ϸ�");
    }
}
