using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Minigame.RGLight;
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
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OmokFirebaseManager : Singleton<OmokFirebaseManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private DatabaseReference _dbRoomRef;
    private DatabaseReference _dbLogInUserDataRef;
    private DatabaseReference _dbOmokUserDataRef;

    public bool amIHost = false;

    private LogInUserData _logInUserData;
    public RoomData currentRoomData;
    public OmokUserData hostData;
    public OmokUserData guestData;

    [SerializeField] private Board board;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
    }

    private bool _isDataLoaded = false; // 데이터 로딩 상태 플래그

    private async void Start()
    {
        try
        {
            Auth = GameManager.Instance.FirebaseManager.Auth;
            Database = GameManager.Instance.FirebaseManager.Database;
            User = GameManager.Instance.FirebaseManager.User;

            _dbLogInUserDataRef = Database.GetReference($"loginusers");

            //로그인한 유저에서 쓸 데이터 -> userid, nickname, gold, servername
            //1. 현재 유저의 정보를 찾고
            _logInUserData = await SetLogInUserDataByUserId(Auth.CurrentUser.UserId);

            //2. 찾은 유저의 정보로 방의 정보 찾고
            currentRoomData = await FindRoomDataByLogInUserData(_logInUserData);

            //3. 찾은 방의 정보로 방 참조 선언
            if (_logInUserData != null && currentRoomData != null)
            {
                _dbRoomRef = Database.GetReference("omokuserdata")
                    .Child("rooms")
                    .Child(_logInUserData.serverName)
                    .Child(currentRoomData.roomKey);
            }
            else
            {
                print("로그인 한 유저의 데이터가 없거나 방의 정보가 없다");
            }

            if (string.IsNullOrEmpty(currentRoomData.roomKey))
            {
                Debug.LogError("방 키를 찾을 수 없습니다.");
                return;
            }

            print($"currentRoomData가 잘 넘어왔습니다 : {currentRoomData}");
            LastTimeHandler.Instance.SetRef(_dbRoomRef);

            //그 넘겨준 정보로 host와 guest 정보를 OmokUserData로 치환함
            hostData = await SetUserData(currentRoomData.host);
            guestData = await SetUserData(currentRoomData.guest);

            amIHost = Auth.CurrentUser.UserId == currentRoomData.host;
            print($"나 호스트임 ? {amIHost}");

            if (amIHost)
            {
                _dbOmokUserDataRef = Database.GetReference("omokuserdata").Child(hostData.id);
                print($"지금 참조하고 있는 유저의 데이터 레퍼런스는 호스트 : {_dbOmokUserDataRef}");
            }
            else
            {
                _dbOmokUserDataRef = Database.GetReference("omokuserdata").Child(guestData.id);
            }

            //강제종료 추적
            await SetupOnDisconnect();

            OmokUIPage omokUIPage = OmokUIManager.Instance.PageOpen<OmokUIPage>();

            omokUIPage.Init(currentRoomData, hostData, guestData, amIHost);

            MonitorTurnList();
            MonitorPlayerExit();

            _isDataLoaded = true;

            AudioManager.Instance.PlayBgm(Bgm.None);
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase방 참조 오류 : {e.Message}");
        }
    }

    private async Task<LogInUserData> SetLogInUserDataByUserId(string id)
    {
        try
        {
            DatabaseReference logInUserDataRef = _dbLogInUserDataRef.Child(id);

            DataSnapshot logInUserDataSnapshot = await logInUserDataRef.GetValueAsync();

            if (logInUserDataSnapshot.Exists)
            {
                string logInUserJson = logInUserDataSnapshot.GetRawJsonValue();
                print($"로그인한 유저의 Json데이터 : {logInUserJson}");
                _logInUserData = JsonConvert.DeserializeObject<LogInUserData>(logInUserJson);
                return _logInUserData;
            }
            else
            {
                Debug.LogWarning("로그인한 유저의 정보가 없습니다!");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"로그인 유저 정보 변환 중 오류 발생 {e.Message}");
            return null;
        }
    }

    private async Task<RoomData> FindRoomDataByLogInUserData(LogInUserData logInUserData)
    {
        try
        {
            //일단 해당 서버에 모든 방을 찾음
            DatabaseReference roomDataRef = Database.GetReference("omokuserdata")
                    .Child("rooms")
                    .Child(_logInUserData.serverName);

            DataSnapshot findRoomsSnapshot = await roomDataRef.GetValueAsync();

            if (!findRoomsSnapshot.Exists)
            {
                Debug.LogWarning("해당 서버에 방이 없음");
                return null;
            }

            //모든 방 중에서 내 아이디가 호스트인지 게스트인지는 모르겠으나 일단 있고, state도 1(Playing)인 방이면 참조할것임
            foreach (DataSnapshot roomSnapshot in findRoomsSnapshot.Children)
            {
                string roomJson = roomSnapshot.GetRawJsonValue();
                RoomData roomData = JsonConvert.DeserializeObject<RoomData>(roomJson);

                if ((roomData.host == logInUserData.id || roomData.guest == logInUserData.id) && (int)roomData.state == 1)
                {
                    Debug.Log($"찾은 방 키: {roomSnapshot.Key}");
                    return roomData;
                }

                else
                {
                    print("너가 원하는 방은 없는데?");
                }
            }
            Debug.LogWarning("유저가 속한 방을 찾을 수 없습니다.");
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError($"방 찾는 중 오류 발생 : {e.Message}");
            return null;
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

    public void RequestPlaceStone(Vector2Int boardIndex)
    {
        if (!_isDataLoaded || currentRoomData == null)
        {
            Debug.LogWarning("currentRoomData가 초기화되지 않았습니다.");
            return;
        }

        //턴이 맞지 않을때는 이곳이 실행됨
        if (!IsMyTurn())
        {
            Debug.Log("내 차례가 아님!");
            return;
        }

        //착수한 기록이 남는 턴 데이터
        Turn newTurn = new Turn
        {
            coodinate = $"{boardIndex.x}, {boardIndex.y}",
            isHostTurn = amIHost,
            turnCount = currentRoomData.turnCount
        };

        AddTurnToFirebase(newTurn);
    }

    //어디에 뒀는지 기록을 남기는 함수
    private async void AddTurnToFirebase(Turn turn)
    {
        if (turn == null)
        {
            Debug.LogError("AddTurnToFirebase: Turn 객체가 null입니다.");
            return;
        }
        try
        {
            DatabaseReference turnListRef = _dbRoomRef.Child("turnList");

            string turnJson = JsonConvert.SerializeObject(turn);

            await turnListRef.Child($"{turn.turnCount}").SetRawJsonValueAsync(turnJson);

            Debug.Log($"{turn.turnCount}번째 수 : {turn.coodinate}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase 턴 참조 오류 {e.Message}");
        }
    }

    //RoomData에 turnList정보가 바뀔 때 마다 실행됨
    private void MonitorTurnList()
    {
        if (_dbRoomRef == null)
        {
            Debug.LogWarning("_dbRoomRef가 아직 초기화되지 않음");
            return;
        }

        if (board == null)
        {
            Debug.LogWarning("board가 아직 초기화되지 않았음. 초기화될 때까지 대기합니다.");
            StartCoroutine(WaitForBoardInitialization());
            return;
        }

        DatabaseReference turnListRef = _dbRoomRef.Child("turnList");

        //turnListRef.ChildAdded += HandleTurnAdded;
        //턴이 바뀌는 하나만 참조함
        turnListRef.OrderByKey()
               .LimitToLast(1)
               .ChildAdded += HandleTurnAdded;
    }

    private IEnumerator WaitForBoardInitialization()
    {
        while (board == null)
        {
            yield return null;
        }

        // board가 초기화된 후 MonitorTurnList 호출
        MonitorTurnList();
    }

    //턴이 바뀌면
    private void HandleTurnAdded(object sender, ChildChangedEventArgs args)
    {
        //if (!args.Snapshot.Exists) return;

        try
        {
            string turnJson = args.Snapshot.GetRawJsonValue();
            Turn newTurn = JsonConvert.DeserializeObject<Turn>(turnJson);

            if (newTurn == null)
            {
                Debug.LogWarning("newTurn이 null입니다");
                return;
            }

            Debug.Log($"현재 턴의 정보 : isHostTurn : {newTurn.isHostTurn}, coodinate = {newTurn.coodinate}");

            string[] split = newTurn.coodinate.Split(",");
            int x = int.Parse(split[0]);
            int y = int.Parse(split[1]);

            board.PlaceStone(newTurn.isHostTurn, new Vector2Int(x, y));
            //_currentRoomData.isHostTurn = !newTurn.isHostTurn;
            //_currentRoomData.turnCount++;

            UpdateRoomData(newTurn.isHostTurn, newTurn.turnCount);

        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase 턴 참조 오류 : {e.Message}");
        }
    }

    private async void UpdateRoomData(bool isHostTurn, int turnCount)
    {
        currentRoomData.isHostTurn = !isHostTurn;
        currentRoomData.turnCount = turnCount + 1;

        //수를 둘 때마다 시간이 흐름
        LastTimeHandler.Instance.HandleTime();

        OmokUIManager.Instance.PageUse<OmokUIPage>().UpdateTurnInfo(currentRoomData.turnCount, IsMyTurn());

        Dictionary<string, object> updateDic = new Dictionary<string, object>();
        updateDic["isHostTurn"] = currentRoomData.isHostTurn;
        updateDic["turnCount"] = currentRoomData.turnCount;

        await _dbRoomRef.UpdateChildrenAsync(updateDic);
    }

    public bool AmIHost()
    {
        if (_isDataLoaded == false)
        {
            Debug.LogWarning("데이터 로딩이 완료되지 않았습니다.");
            return false;
        }
        print($"currentRoomData : {currentRoomData}");
        print($"currentRoomData.host : {currentRoomData.host}");
        string myUserId = GameManager.Instance.FirebaseManager.User.UserId;
        return myUserId == currentRoomData.host;
    }

    private bool _isHostTurn;
    public bool IsMyTurn()
    {
        bool isMyTurn;
        if ((currentRoomData.turnCount % 2 == 0 && amIHost == true) || (currentRoomData.turnCount % 2 == 1 && amIHost == false))
        {
            isMyTurn = true;
        }
        else
        {
            isMyTurn = false;
        }
        return isMyTurn;
    }

    //여기에서 리더보드와 omokuserdata 둘 다 정보를 업데이트해줌
    public async void UpdateUserResult(OmokUserData userData, bool isWin)
    {
        //OmokUserData 경로
        DatabaseReference userDataRef = Database.GetReference("omokuserdata").Child(userData.id);

        //리더보드 경로
        DatabaseReference omokLeaderBoardRef = Database.GetReference("leaderboard")
            .Child("omok")
            .Child(userData.id);

        if (isWin)
        {
            userData.win++;
            userData.gold += currentRoomData.betting;
        }
        else
        {
            userData.lose++;
            userData.gold -= currentRoomData.betting;
        }

        //OmokUserData에 정보 업데이트
        string updatedUserDataJson = JsonConvert.SerializeObject(userData);
        await userDataRef.SetRawJsonValueAsync(updatedUserDataJson);

        //리더보드에 정보 업데이트
        OmokLeaderBoardData leaderboardData = new OmokLeaderBoardData()
        {
            nickname = userData.nickname,
            win = userData.win,
            lose = userData.lose,
            score = userData.win * 10 - userData.lose * 5
        };

        string leaderJson = JsonConvert.SerializeObject(leaderboardData);
        await omokLeaderBoardRef.SetRawJsonValueAsync(leaderJson);
    }

    //플레이어가 나가는걸 다룸(항복 & 강제종료)
    private void MonitorPlayerExit()
    {
        if (_dbRoomRef == null)
        {
            Debug.LogWarning("MonitorPlayerExit: _dbRoomRef가 null입니다.");
            return;
        }

        // 기존 이벤트 핸들러 제거
        _dbRoomRef.Child("hostExited").ValueChanged -= OnHostExited;
        _dbRoomRef.Child("guestExited").ValueChanged -= OnGuestExited;

        // 새로운 이벤트 핸들러 등록
        _dbRoomRef.Child("hostExited").ValueChanged += OnHostExited;
        _dbRoomRef.Child("guestExited").ValueChanged += OnGuestExited;
    }

    private bool _isHostExitedProcessed = false;
    private bool _isGuestExitedProcessed = false;

    private void OnHostExited(object sender, ValueChangedEventArgs args)
    {
        if (args.Snapshot.Exists && args.Snapshot.Value is bool isHostExited && isHostExited)
        {
            if (_isHostExitedProcessed)
            {
                Debug.Log("호스트 나가기 이벤트가 이미 처리되었습니다.");
                return;
            }

            _isHostExitedProcessed = true; // 중복 처리 방지 플래그 설정
            Debug.Log("호스트가 게임을 나갔습니다. 승리 처리 중...");

            if (amIHost == false)
            {
                UpdateUserResult(guestData, true);
                UpdateUserResult(hostData, false);
                OmokOneButtonPopup omokOneButtonPopup = OmokUIManager.Instance.PopupOpen<OmokOneButtonPopup>();
                omokOneButtonPopup.SetPopup(true, currentRoomData.betting);
                //HandlePlayerExit(true); // 게스트가 승리
            }
        }
    }

    private void OnGuestExited(object sender, ValueChangedEventArgs args)
    {
        if (args.Snapshot.Exists && args.Snapshot.Value is bool isGuestExited && isGuestExited)
        {
            if (_isGuestExitedProcessed)
            {
                Debug.Log("게스트 나가기 이벤트가 이미 처리되었습니다.");
                return;
            }

            _isGuestExitedProcessed = true; // 중복 처리 방지 플래그 설정
            Debug.Log("게스트가 게임을 나갔습니다. 승리 처리 중...");

            if (amIHost == true)
            {
                UpdateUserResult(hostData, true);
                UpdateUserResult(guestData, false);
                OmokOneButtonPopup omokOneButtonPopup = OmokUIManager.Instance.PopupOpen<OmokOneButtonPopup>();
                omokOneButtonPopup.SetPopup(true, currentRoomData.betting);
                //HandlePlayerExit(false); // 호스트가 승리
            }
        }
    }

    //강제종료
    private async Task SetupOnDisconnect()
    {
        if (_dbRoomRef == null)
        {
            Debug.LogWarning("방 참조가 연결이 안됨");
            return;
        }

        string playerKey = amIHost ? "hostExited" : "guestExited";

        DatabaseReference exitRef = _dbRoomRef.Child(playerKey);

        OnDisconnect onDisconnect = exitRef.OnDisconnect();
        await onDisconnect.SetValue(true);

        Debug.Log($"OnDisconnect 설정 완료: {playerKey} -> true");
    }

    public string nextSceneName = "YooLobby";

    public async void ExitGame(bool isSurrender)
    {
        Debug.Log($"ExitGame 호출됨, isSurrender: {isSurrender}");
        try
        {
            string playerKey = amIHost ? "hostExited" : "guestExited";

            //항복이면 상대방의 승리를 입력해줘야한다
            if (isSurrender)
            {
                //UpdateOmokUserData(false);
                //HandlePlayerExit(amIHost);
                OmokUserData loserData = amIHost ? hostData : guestData;
                OmokUserData winnerData = amIHost ? guestData : hostData;

                UpdateUserResult(loserData, false);
                UpdateUserResult(winnerData, true);

                //여기에서 hostExited, guestExited를 true로 바꿔줌
                await _dbRoomRef.Child(playerKey).SetValueAsync(true);
            }

            await _dbRoomRef.Child("state").SetValueAsync((int)RoomState.Finished);
            SceneManager.LoadScene(nextSceneName);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"호스트 게스트 존재가 확인되지 않음 : {e.Message}");
        }
    }
}
