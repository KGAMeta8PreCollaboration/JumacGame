using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;

public class LobbyManager : MonoBehaviour
{
    public LocalPlayer myLobbyPlayer;
    [HideInInspector] public LogInUserData logInUserData;

    [SerializeField] private GameObject myPlayerPrefab;
    [SerializeField] private RemotePlayer otherPlayerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    private DatabaseReference _dbLobbyRef;
    private Dictionary<string, RemotePlayer> otherLobbyPlayerDic = new Dictionary<string, RemotePlayer>();
    private DatabaseReference userListRef;
    
    public async void Init()
    {
        DataSnapshot data = await GameManager.Instance.FirebaseManager.Database.GetReference(
            $"loginusers/{GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId}").GetValueAsync();
        logInUserData = JsonConvert.DeserializeObject<LogInUserData>(data.GetRawJsonValue());
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (status == DependencyStatus.Available)
        {
            _dbLobbyRef = GameManager.Instance.FirebaseManager.Database.GetReference("lobby");
        }
        JoinLobby(logInUserData.serverName, logInUserData.nickname);
    }

    public void CreateLobby(string lobbyName, string username)
    {
        LobbyData lobbyData = new LobbyData(lobbyName, username);
        string str = JsonConvert.SerializeObject(lobbyData);
        _dbLobbyRef.Child(lobbyName).SetRawJsonValueAsync(str);
    }

    private void CreatePlayer(string uid, string nickname, Vector3 position, string race)
    {
        RemotePlayer player = Instantiate(otherPlayerPrefab, position, Quaternion.identity);
        player.Init(uid, nickname, position, race);
        otherLobbyPlayerDic.Add(uid, player);
    }

    private void CreateMyPlayer(string uid, string nickname, Vector3 position)
    {
        LocalPlayer player = 
            Instantiate(myPlayerPrefab, playerSpawnPoint.position, Quaternion.identity)
                .GetComponentInChildren<LocalPlayer>();
        player.Init(uid, nickname, logInUserData.race);
        myLobbyPlayer = player;
    }

    public async void JoinLobby(string lobbyName, string username)
    {
        if (_dbLobbyRef == null)
        {
            Debug.LogWarning("DBReference is null");
            return;
        }
        DataSnapshot data = await _dbLobbyRef.Child(lobbyName).GetValueAsync();
        LobbyData lobbyData;
        // 로비 정보가 있을때
        if (data.Exists)
        {
            lobbyData = JsonConvert.DeserializeObject<LobbyData>(data.GetRawJsonValue());
            CreateMyPlayer(logInUserData.id, username, Vector3.up);
        }
        else
            CreateLobby(lobbyName, username);
        userListRef = _dbLobbyRef.Child(logInUserData.serverName).Child("userlist");
        userListRef.Child(logInUserData.id).OnDisconnect().RemoveValue();
        userListRef.ChildChanged += OnChildMoved;
        userListRef.ChildAdded += OnChildAdded;
        userListRef.ChildRemoved += OnChildRemoved;
    }

    private void OnDisable()
    {
        OnQuit();
    }


    public void OnQuit()
    {
        userListRef.ChildChanged -= OnChildMoved;
        userListRef.ChildAdded -= OnChildAdded;
        userListRef.ChildRemoved -= OnChildRemoved;
        userListRef.Child(logInUserData.id).RemoveValueAsync();
    }
    
    // private void OnApplicationQuit()
    // {
    //     OnQuit();
    // }
    
    private void OnChildRemoved(object sender, ChildChangedEventArgs e)
    {
        if (otherLobbyPlayerDic.ContainsKey(e.Snapshot.Key))
        {
            Destroy(otherLobbyPlayerDic[e.Snapshot.Key].gameObject);
            otherLobbyPlayerDic.Remove(e.Snapshot.Key);
        }
    }
    private async void OnChildAdded(object sender, ChildChangedEventArgs e)
    {
        if (e.Snapshot.Key == logInUserData.id)
        {
            return;
        }
        LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(e.Snapshot.GetRawJsonValue());
        DataSnapshot data = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child(e.Snapshot.Key).GetValueAsync();
        string a = data.GetRawJsonValue();
        CreatePlayer(e.Snapshot.Key, user.username, new Vector3(user.position.x, 1, user.position.z), data.Child("race").Value.ToString());
    }

    private void Start()
    {
        Init();

        StartCoroutine(SendPositionCoroutine());
    }
    private void OnChildMoved(object sender, ChildChangedEventArgs e)
    {
        if (e.Snapshot.Key == logInUserData.id)
            return;
        if (otherLobbyPlayerDic.ContainsKey(e.Snapshot.Key))
        {
            LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(e.Snapshot.GetRawJsonValue());
            otherLobbyPlayerDic[e.Snapshot.Key].SetPosition(new Vector3(user.position.x, user.position.y, user.position.z));
        }
        else
        {
            Debug.LogError("플레이어 없는데 움직이네..?");
            LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(e.Snapshot.GetRawJsonValue());
            // CreatePlayer(e.Snapshot.Key, user.username, new Vector3(user.position.x, user.position.y, user.position.z));
        }
    }

    private IEnumerator SendPositionCoroutine()
    {
        while (true)
        {
            SendMyPosition();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SendMyPosition()
    {
        if (myLobbyPlayer == null)
            return;
        
        LobbyData.User user = new LobbyData.User(myLobbyPlayer.username,
            new Vector3(myLobbyPlayer.transform.position.x, 
                    myLobbyPlayer.transform.position.y, 
                    myLobbyPlayer.transform.position.z));
        string str = JsonConvert.SerializeObject(user);
        _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").Child(logInUserData.id).SetRawJsonValueAsync(str);
    }
}

public class Point
{
    public float x;
    public float y;
    public float z;

    public Point()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public Point(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

