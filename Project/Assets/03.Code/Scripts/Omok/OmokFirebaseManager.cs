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

public class OmokFirebaseManager : Singleton<OmokFirebaseManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private LogInUserData _logInUserData;
    private DatabaseReference _dbRoomRef;

    public RoomData currentRoomData;
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
                currentRoomData = JsonConvert.DeserializeObject<RoomData>(roomDataJson);
                _dbRoomRef = Database.GetReference(currentRoomData.serverName)
                    .Child("rooms")
                    .Child(currentRoomData.roomKey);

                LastTimeHandler.Instance.SetRef(_dbRoomRef);

                //그 넘겨준 정보로 host와 guest 정보를 OmokUserData로 치환함
                hostData = await SetUserData(currentRoomData.host);
                guestData = await SetUserData(currentRoomData.guest);
            }
            MonitorTurnList();
            OmokUIManager.Instance.PageOpen<OmokUIPage>().Init(currentRoomData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase방 참조 오류 : {e.Message}");
        }
    }

    //id를 OmokUserData로 치환해주는 함수
    private async Task<OmokUserData> SetUserData(string id)
    {
        try
        {
            DatabaseReference omokDataRef = Database.GetReference("omokuserdata")
                .Child(id);

            DataSnapshot omokDataSnapshot = await omokDataRef.GetValueAsync();

            //스냅샷이 존재하면 그대로 끌어다 쓰면 된다
            if (omokDataSnapshot.Exists)
            {
                string omokUserJson = omokDataSnapshot.GetRawJsonValue();
                OmokUserData omokUserData = JsonConvert.DeserializeObject<OmokUserData>(omokUserJson);


                return omokUserData;
            }
            //존재하지 않으면 UserId만 존재하는 OmokUSerData로 바꿔준다
            else
            {
                DatabaseReference lobbyUserData = Database.GetReference("loginusers")
                    .Child(id);
                DataSnapshot lobbyUserDataSnapshot = await lobbyUserData.GetValueAsync();
                string lobbyUserDataJson = lobbyUserDataSnapshot.GetRawJsonValue();
                LogInUserData logInUserData = JsonConvert.DeserializeObject<LogInUserData>(lobbyUserDataJson);

                OmokUserData newOmokUserData = new OmokUserData(id, logInUserData.nickname, logInUserData.gold);
                return newOmokUserData;
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

    public void RequestPlaceStone(Vector2Int boardIndex)
    {
        bool amIHost = Auth.CurrentUser.UserId == currentRoomData.host;

        //턴이 맞지 않을때는 이곳이 실행됨
        if (currentRoomData.isHostTurn && !amIHost)
        {
            Debug.Log("현재 호스트 턴인데, 나는 게스트!.");
            return;
        }
        if (!currentRoomData.isHostTurn && amIHost)
        {
            Debug.Log("현재 게스트 턴인데, 나는 호스트!.");
            return;
        }

        //_turnCount++;

        Turn newTurn = new Turn
        {
            coodinate = $"{boardIndex.x}, {boardIndex.y}",
            isHostTurn = amIHost,
            turnCount = currentRoomData.turnCount //여기에서 turnCount = 1
        };

        //board.PlaceStone(newTurn.isHostTurn, new Vector2Int(boardIndex.x, boardIndex.y));
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

                //_currentRoomData.isHostTurn = !newTurn.isHostTurn;
                //_currentRoomData.turnCount++;

                UpdateRoomData(newTurn.isHostTurn, newTurn.turnCount);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"Firebase 턴 참조 오류 : {e.Message}");
        }
    }

    private async void UpdateRoomData(bool isHostTurn, int turnCount)
    {
        currentRoomData.isHostTurn = !isHostTurn;
        currentRoomData.turnCount = turnCount + 1;

        LastTimeHandler.Instance.HandleTime();

        OmokUIManager.Instance.PageUse<OmokUIPage>().UpdateTurnInfo(currentRoomData.turnCount + 1);

        Dictionary<string, object> updateDic = new Dictionary<string, object>();
        updateDic["isHostTurn"] = currentRoomData.isHostTurn;
        updateDic["turnCount"] = currentRoomData.turnCount;

        await _dbRoomRef.UpdateChildrenAsync(updateDic);
    }

    public bool AmIHost()
    {
        string myUserId = GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId;
        return myUserId == currentRoomData.host;
    }

    public async void UpdateOmokUserData(bool amIWin)
    {
        //리더보드 정보 최신화
        DatabaseReference omokLeaderBoardRef = Database.GetReference("leaderboard")
            .Child("omok")
            .Child(Auth.CurrentUser.UserId);

        OmokUserData myOmokData = currentRoomData.host == Auth.CurrentUser.UserId ? hostData : guestData;

        //omokuser정보 최신화
        DatabaseReference myOmokDataRef = Database.GetReference("omokuserdata")
            .Child(myOmokData.id);

        OmokLeaderBoardData omokLeaderBoardData = new OmokLeaderBoardData();
        //이겼을때
        if (amIWin == true)
        {
            print("승리점수 얻음");
            omokLeaderBoardData.nickName = myOmokData.nickname;
            omokLeaderBoardData.win = myOmokData.win + 1;
            omokLeaderBoardData.lose = myOmokData.lose;

            myOmokData.win++;
        }
        else
        {
            print("패배점수 얻음");
            omokLeaderBoardData.nickName = myOmokData.nickname;
            omokLeaderBoardData.win = myOmokData.win;
            omokLeaderBoardData.lose = myOmokData.lose + 1;

            myOmokData.lose++;
        }

        string omokLeaderBoardDataJson = JsonConvert.SerializeObject(omokLeaderBoardData);
        await omokLeaderBoardRef.SetRawJsonValueAsync(omokLeaderBoardDataJson);

        string myOmokDataJson = JsonConvert.SerializeObject(myOmokData);
        await myOmokDataRef.SetRawJsonValueAsync(myOmokDataJson);
    }

    public async void ExitGame()
    {
        DatabaseReference stateRef = _dbRoomRef.Child($"state");
        await _dbRoomRef.Child($"state").SetRawJsonValueAsync(((int)RoomState.Finished).ToString());
    }
}
