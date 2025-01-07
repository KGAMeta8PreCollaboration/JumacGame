using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;

public class FirebaseTest : MonoBehaviour
{
	public LobbyPlayer playerPrefab;
	
	public FirebaseApp App { get; private set; }
	public FirebaseAuth Auth { get; private set; }
	public FirebaseDatabase DB { get; private set; }
	public DatabaseReference DBUserRef;
	public DatabaseReference DBRoomRef;
	public List<LobbyPlayer> lobbyPlayers = new List<LobbyPlayer>();

	private string myTableName = "b";
	
	private async void Start()
	{
		DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
		// print($"status : {status}");
		// 초기화 성공
		if (status == DependencyStatus.Available)
		{
			// print($"Firebase Init 1");
			App = FirebaseApp.DefaultInstance;
			Auth = FirebaseAuth.DefaultInstance;
			DB = FirebaseDatabase.DefaultInstance;
			
			DatabaseReference reference = DB.GetReference(myTableName);
			DBUserRef = reference.Child("Users");
			DBRoomRef = reference.Child("Rooms");
			// DBUserRef = DB.GetReference("Users");
			// DBRoomRef = DB.GetReference("Rooms");
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
			
			if (DBRoomRef != null)
			{
				// print($"Rooms Init");
				string roomName = "Tmp";
				DataSnapshot data = await DBRoomRef.Child(roomName).GetValueAsync();
				Lobby lobby;
				// 룸 정보가 있을 경우
				if (data.Exists)
				{
					lobby = JsonConvert.DeserializeObject<Lobby>(data.GetRawJsonValue());
					for (int i = 0; i < lobby.userList.Count; i++)
					{
						LobbyPlayer fieldPlayer = Instantiate(playerPrefab);
						fieldPlayer.gameObject.name = lobby.userList[i].username;
						fieldPlayer.username = lobby.userList[i].username;
						fieldPlayer.position = new Vector3(lobby.userList[i].position.x, lobby.userList[i].position.y, lobby.userList[i].position.z);
						lobbyPlayers.Add(fieldPlayer);
					}
				}
				else
				{
					lobby = new Lobby(roomName, "test");
					string str = JsonConvert.SerializeObject(lobby);
					DBRoomRef.Child(roomName).SetRawJsonValueAsync(str);
				}
				DBRoomRef.Child(roomName).Child("userList").ValueChanged += OnMoved;
				// DBRoomRef.Child(roomName).Child("userList").ChildChanged += OnChildMoved;
			}
			else
			{
				Debug.LogWarning("DBReference is null");
			}

		}

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
public class User
{
	public string username;
	public string email;
	public string password;
	
	public User()
	{
		username = "username";
		email = "email";
		password = "password";
	}

	public User(string username, string email, string password)
	{
		this.username = username;
		this.email = email;
		this.password = password;
	}
}

public class Lobby
{
	public class User
	{
		public string username;
		public Point position;

		public User()
		{
			username = "username";
			position = new Point(0,0,0);
		}
		
		public User(string username)
		{
			this.username = username;
			position = new Point(0,0,0);
		}
		
		public User(string username, Vector3 pos)
		{
			this.username = username;
			position = new Point(pos.x,pos.y,pos.z);
		}

	}
	public string lobbyName;
	public string roomOwner;
	public List<User> userList = new List<User>();

	public Lobby()
	{
		lobbyName = "roomName";
		roomOwner = "roomOwner";
	}

	public Lobby(string lobbyName, string roomOwner)
	{
		this.lobbyName = lobbyName;
		this.roomOwner = roomOwner;
		userList.Add(new User(lobbyName));
	}

	public void AddUser(string username, Vector3 position)
	{
		userList.Add(new User(username, position));
	}
}
