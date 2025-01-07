using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Serialization;

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
		// 초기화 성공
		if (status == DependencyStatus.Available)
		{
			App = FirebaseApp.DefaultInstance;
			Auth = FirebaseAuth.DefaultInstance;
			DB = FirebaseDatabase.DefaultInstance;

			DBUserRef = DB.GetReference("Users");
			DBRoomRef = DB.GetReference("Rooms");
			if (DBUserRef != null)
			{
				User user = new User("test", "1234", "1234");
				string str = JsonConvert.SerializeObject(user);
				DBUserRef.Child(user.username).SetRawJsonValueAsync(str);
			}
			else
			{
				Debug.LogWarning("DBReference is null");
			}
			
			if (DBRoomRef != null)
			{
				string roomName = "Tmp";

				// Room room = new Room(roomName, "1q2w3e4r", "testUser");
				// room.AddUser("testUser2", new Vector3(5, 2, 5));
				// string str = 
					// JsonConvert.SerializeObject(room);
				
				// DBRoomRef.Child(roomName).SetRawJsonValueAsync(str);

				DataSnapshot data = await DBRoomRef.Child(roomName).GetValueAsync();
				Room room;
				if (data.Exists)
				{
					room = JsonConvert.DeserializeObject<Room>(data.GetRawJsonValue());
					for (int i = 0; i < room.userList.Count; i++)
					{
						LobbyPlayer fieldPlayer = Instantiate(playerPrefab);
						fieldPlayer.gameObject.name = room.userList[i].username;
						fieldPlayer.username = room.userList[i].username;
						fieldPlayer.position = new Vector3(room.userList[i].position.x, room.userList[i].position.y, room.userList[i].position.z);
						lobbyPlayers.Add(fieldPlayer);
					}
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
		// print($"child sender :|{sender}");
		// print($"child sender :|{sender.ToString()}|");
		foreach (DataSnapshot child in e.Snapshot.Children)
		{
			Room.User tmp = JsonConvert.DeserializeObject<Room.User>(child.GetRawJsonValue());
			// print($"c name : {tmp.username}");
			// print($"c pos : {tmp.position}");
			lobbyPlayers.Find(x => x.username == tmp.username).position = new Vector3(tmp.position.x, tmp.position.y, tmp.position.z);
			// print($"c : {child.GetRawJsonValue()}");
		}
	}
	
	void OnMoved(object sender, ValueChangedEventArgs e)
	{
		// print($"parent sender :|{sender}");
		// print($"parent sender :|{sender.ToString()}|");
		// print($"{}");
		foreach (DataSnapshot child in e.Snapshot.Children)
		{
			Room.User tmp = JsonConvert.DeserializeObject<Room.User>(child.GetRawJsonValue());
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

public class Room
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
	public string roomName;
	public string roomPassword;
	public string roomOwner;
	public List<User> userList = new List<User>();

	public Room()
	{
		roomName = "roomName";
		roomPassword = "roomPassword";
		roomOwner = "roomOwner";
	}

	public Room(string roomName, string roomPassword, string roomOwner)
	{
		this.roomName = roomName;
		this.roomPassword = roomPassword;
		this.roomOwner = roomOwner;
		userList.Add(new User(roomName));
	}

	public void AddUser(string username, Vector3 position)
	{
		userList.Add(new User(username, position));
	}
}
