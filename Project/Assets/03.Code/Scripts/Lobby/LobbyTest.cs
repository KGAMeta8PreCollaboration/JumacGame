using UnityEngine;
using Firebase.Auth;
using Firebase.Database;

public class LobbyTest : MonoBehaviour
{
	private FirebaseAuth _auth;
	private FirebaseDatabase _db;

	private void Start()
	{
		_auth = FirebaseAuth.DefaultInstance;
		_db = FirebaseDatabase.DefaultInstance;
		_auth.CreateUserWithEmailAndPasswordAsync("bbb@bbb.bbb", "bbbbbb");
	}
}
