using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.AI;

public class LobbyManager : MonoBehaviour
{
    public LobbyPlayer myLobbyPlayer;
    [HideInInspector] public LogInUserData logInUserData;

    [SerializeField] private LobbyPlayer myPlayerPrefab;
    [SerializeField] private LobbyPlayer otherPlayerPrefab;

    private DatabaseReference _dbLobbyRef;
    private Dictionary<string, LobbyPlayer> otherLobbyPlayerDic = new Dictionary<string, LobbyPlayer>();
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
            //_dbLobbyRef = reference.Child("lobby");
            //_dbLobbyRef = reference;
        }
        JoinLobby(logInUserData.serverName, logInUserData.nickname);
    }

    public void CreateLobby(string lobbyName, string username)
    {
        LobbyData lobbyData = new LobbyData(lobbyName, username);
        string str = JsonConvert.SerializeObject(lobbyData);
        _dbLobbyRef.Child(lobbyName).SetRawJsonValueAsync(str);
    }

    public void AddUser(string lobbyName, string uid, string username, Vector3 position)
    {
        LobbyData.User user = new LobbyData.User(username, position);
        string str = JsonConvert.SerializeObject(user);
        _dbLobbyRef.Child(lobbyName).Child("userlist").Child(uid).SetRawJsonValueAsync(str);
    }

    // private void CreateMyPlayer()
    // {
    // 	myLobbyPlayer = Instantiate(playerPrefab);
    // 	myLobbyPlayer.UID = logInUserData.id;
    // 	myLobbyPlayer.username = logInUserData.nickname;
    // }

    private void CreatePlayer(string uid, string nickname, Vector3 position)
    {
        LobbyPlayer player = Instantiate(otherPlayerPrefab, position, Quaternion.identity);
        player.UID = uid;
        player.username = nickname;
        player.position = position;
        otherLobbyPlayerDic.Add(uid, player);
    }

    private void CreateMyPlayer(string uid, string nickname, Vector3 position)
    {
        print("CreateMyPlayer");
        LobbyPlayer player = Instantiate(myPlayerPrefab);
        player.UID = uid;
        player.username = nickname;
        player.position = position;
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
            CreateMyPlayer(logInUserData.id, username, Vector3.up);
            // CreatePlayer(logInUserData.id, username, Vector3.zero);
            lobbyData = JsonConvert.DeserializeObject<LobbyData>(data.GetRawJsonValue());
        }
        else
            CreateLobby(lobbyName, username);
        // _dbLobbyRef.Child(lobbyName).Child("userList").ValueChanged += OnMoved;

        // _dbLobbyRef.Child(lobbyName).Child("userList").Child(logInUserData.id).OnDisconnect().RemoveValue();
        userListRef = _dbLobbyRef.Child(logInUserData.serverName).Child("userlist");
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").Child(logInUserData.id).OnDisconnect().RemoveValue();
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").ChildChanged += OnChildMoved;
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").ChildAdded += OnChildAdded;
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").ChildRemoved += OnChildRemoved;
        userListRef.Child(logInUserData.id).OnDisconnect().RemoveValue();
        userListRef.ChildChanged += OnChildMoved;
        userListRef.ChildAdded += OnChildAdded;
        userListRef.ChildRemoved += OnChildRemoved;
    }

    private void OnDestroy()
    {
        OnQuit();
    }


    public void OnQuit()
    {
        print("OnQuit");
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").ChildChanged -= OnChildMoved;
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").ChildAdded -= OnChildAdded;
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").ChildRemoved -= OnChildRemoved;
        // _dbLobbyRef.Child(logInUserData.serverName).Child("userlist").Child(logInUserData.id).RemoveValueAsync();
        userListRef.ChildChanged -= OnChildMoved;
        userListRef.ChildAdded -= OnChildAdded;
        userListRef.ChildRemoved -= OnChildRemoved;
        userListRef.Child(logInUserData.id).RemoveValueAsync();
    }

    private void OnApplicationQuit()
    {
        OnQuit();
    }

    private void OnChildRemoved(object sender, ChildChangedEventArgs e)
    {
        if (otherLobbyPlayerDic.ContainsKey(e.Snapshot.Key))
        {
            Destroy(otherLobbyPlayerDic[e.Snapshot.Key].gameObject);
            otherLobbyPlayerDic.Remove(e.Snapshot.Key);
        }
        // print($"OnChildRomoved : key : {e.Snapshot.Key}\n" + $"value : {e.Snapshot.GetRawJsonValue()}\n" + "OnChildRomoved end");
        // throw new System.NotImplementedException();
    }
    private void OnChildAdded(object sender, ChildChangedEventArgs e)
    {
        // print("OnChildAdded start");
        if (e.Snapshot.Key == logInUserData.id)
        {
            return;
        }
        // print($"OnChildAdded : key : {e.Snapshot.Key}\n" + $"value : {e.Snapshot.GetRawJsonValue()}\n" + "OnChildAdded end");
        LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(e.Snapshot.GetRawJsonValue());
        CreatePlayer(e.Snapshot.Key, user.username, new Vector3(user.position.x, 1, user.position.z));
        // print($"user : {user.username} , {user.position}");
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
        print("OnChildMoved");
        // print($"OnChildMoved : key : {e.Snapshot.Key}\n" + $"value : {e.Snapshot.GetRawJsonValue()}\n" + "OnChildMoved end");
        if (otherLobbyPlayerDic.ContainsKey(e.Snapshot.Key))
        {
            LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(e.Snapshot.GetRawJsonValue());
            otherLobbyPlayerDic[e.Snapshot.Key].SetPosition(new Vector3(user.position.x, user.position.y, user.position.z));
            // otherLobbyPlayerDic[e.Snapshot.Key].position = new Vector3(user.position.x, user.position.y, user.position.z);
        }
        else
        {
            LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(e.Snapshot.GetRawJsonValue());
            CreatePlayer(e.Snapshot.Key, user.username, new Vector3(user.position.x, user.position.y, user.position.z));
        }

        // foreach (DataSnapshot child in e.Snapshot.Children)
        // {
        // 	try
        // 	{
        // 		print($"OnChildMoved child : {child}");
        // 		print($"OnChildMoved child getraw : {child.GetRawJsonValue()}");
        // 		LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(child.GetRawJsonValue());
        // 		if (otherLobbyPlayerDic.TryGetValue(child.Key, out LobbyPlayer value))
        // 			value.position = new Vector3(user.position.x, user.position.y, user.position.z);
        // 	}
        // 	catch (JsonSerializationException ex)
        // 	{
        // 		Debug.LogError($"Error deserializing child: {child.GetRawJsonValue()}");
        // 		Debug.LogError(ex);
        // 	}
        // }
    }

    private void OnMoved(object sender, ValueChangedEventArgs e)
    {
        // 생성 처리

        // 변경 처리
        foreach (DataSnapshot child in e.Snapshot.Children)
        {
            LobbyData.User user = JsonConvert.DeserializeObject<LobbyData.User>(child.GetRawJsonValue());
            if (otherLobbyPlayerDic.TryGetValue(child.Key, out LobbyPlayer value))
                value.position = new Vector3(user.position.x, user.position.y, user.position.z);
        }

        // 삭제 처리
    }

    private IEnumerator SendPositionCoroutine()
    {
        while (true)
        {
            SendMyPosition();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SendMyPosition()
    {
        if (myLobbyPlayer == null)
            return;
        LobbyData.User user = new LobbyData.User(myLobbyPlayer.username,
            new Vector3(myLobbyPlayer.position.x, myLobbyPlayer.position.y, myLobbyPlayer.position.z));
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

