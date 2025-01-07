using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class FirebaseTest : MonoBehaviour
{
	public LobbyPlayer playerPrefab;
	
	public FirebaseApp App { get; private set; }
	public FirebaseAuth Auth { get; private set; }
	public FirebaseDatabase DB { get; private set; }
	public DatabaseReference DBUserRef;
	public DatabaseReference DBLobbyRef;
	public List<LobbyPlayer> lobbyPlayers = new List<LobbyPlayer>();
	
	public Button joinButton;

	private string myTableName = "b";

	public async void Init()
	{
		joinButton.onClick.AddListener(() =>
		{
			JoinLobby("Tmp", "test");
		});
		DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
		// print($"status : {status}");
		// 초기화 성공
		print($"status : {status}");
		if (status == DependencyStatus.Available)
		{
			App = FirebaseApp.DefaultInstance;
			Auth = FirebaseAuth.DefaultInstance;
			DB = FirebaseDatabase.DefaultInstance;

			DatabaseReference reference = DB.GetReference(myTableName);
			DBUserRef = reference.Child("Users");
			DBLobbyRef = reference.Child("Lobby");
		}
	}

	public void CreateLobby(string lobbyName, string username)
	{
		Lobby lobby = new Lobby(lobbyName, username);
		string str = JsonConvert.SerializeObject(lobby);
		DBLobbyRef.Child(lobbyName).SetRawJsonValueAsync(str);
	}
	
	public async void JoinLobby(string lobbyName, string username)
	{
		// Lobby lobby = new Lobby(lobbyName, username);
		// string str = JsonConvert.SerializeObject(lobby);
		// DBLobbyRef.Child(lobbyName).SetRawJsonValueAsync(str);
		print($"2 : {DBLobbyRef}");

		if (DBLobbyRef == null)
		{
			Debug.LogWarning("DBReference is null");
			return;
		}
		
		DataSnapshot data = await DBLobbyRef.Child(lobbyName).GetValueAsync();
		Lobby lobby;
		// 로비 정보가 있을때
		if (data.Exists)
		{
			lobby = JsonConvert.DeserializeObject<Lobby>(data.GetRawJsonValue());
			for (int i = 0; i < lobby.userList.Count; i++)
			{
				LobbyPlayer lobbyPlayer = Instantiate(playerPrefab);
				lobbyPlayer.gameObject.name = lobby.userList[i].username;
				lobbyPlayer.username = lobby.userList[i].username;
				lobbyPlayer.position = new Vector3(lobby.userList[i].position.x, lobby.userList[i].position.y, lobby.userList[i].position.z);
				lobbyPlayers.Add(lobbyPlayer);
			}
		}
		else
		{
			CreateLobby(lobbyName, username);
		}
		DBLobbyRef.Child(lobbyName).Child("userList").ValueChanged += OnMoved;
		// DBRoomRef.Child(lobbyName).Child("userList").ChildChanged += OnChildMoved;
	}
	
	private void Start()
	{
		Init();
		// JoinLobby("Tmp","test");
		// DBUserRef = DB.GetReference("Users");
		// DBRoomRef = DB.GetReference("Lobby");
		// if (DBUserRef != null)
		// {
		// 	User user = new User("test", "1234", "1234");
		// 	string str = JsonConvert.SerializeObject(user);
		// 	DBUserRef.Child(user.username).SetRawJsonValueAsync(str);
		// }
		// else
		// {
		// 	Debug.LogWarning("DBReference is null");
		// }
	}
	void OnChildMoved(object sender, ChildChangedEventArgs e)
	{
		print($"child sender :|{sender}");
		print($"child sender :|{sender.ToString()}|");
		foreach (DataSnapshot child in e.Snapshot.Children)
		{
			Lobby.User tmp = JsonConvert.DeserializeObject<Lobby.User>(child.GetRawJsonValue());
			// print($"c name : {tmp.username}");
			// print($"c pos : {tmp.position}");
			lobbyPlayers.Find(x => x.username == tmp.username).position = new Vector3(tmp.position.x, tmp.position.y, tmp.position.z);
			// print($"c : {child.GetRawJsonValue()}");
		}
	}
	
	void OnMoved(object sender, ValueChangedEventArgs e)
	{
		print($"parent sender :|{sender}");
		// print($"parent sender :|{sender.ToString()}|");
		print($"{e}");
		print($"{e.Snapshot}");
		print($"{e.Snapshot.Children}");
		foreach (DataSnapshot child in e.Snapshot.Children)
		{
			Lobby.User tmp = JsonConvert.DeserializeObject<Lobby.User>(child.GetRawJsonValue());
			// print($"p name : {tmp.username}");
			// print($"p pos : {tmp.position.x}, {tmp.position.y}, {tmp.position.z}");
			lobbyPlayers.Find(x => x.username == tmp.username).position = new Vector3(tmp.position.x, tmp.position.y, tmp.position.z);
		}
	}

	// Update is called once per frame
	void Update()
	{

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

