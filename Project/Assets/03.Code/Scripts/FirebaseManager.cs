using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine.UI;

public class FirebaseManager : Singleton<FirebaseManager>
{
	private FirebaseAuth _auth;
	private FirebaseDatabase _database;
	private FirebaseUser _user;

	[SerializeField] private Text _signInInfo;

	private async void Start()
	{
		try
		{
			await FirebaseApp.CheckAndFixDependenciesAsync();

			_auth = FirebaseAuth.DefaultInstance;
			_database = FirebaseDatabase.DefaultInstance;

			//임시로 로그아웃 했음. 에디터에서 멈추고 해도 그 전 값이 있어서 그럼.
			//추후에 그냥 구글 연동이나 게스트 로그인은 로그인 화면을 거치지 않고 바로 갈꺼니 지우기만 하면 됌.
			SignOut();

			_auth.StateChanged += AuthStateChanged;
		}
		catch (Exception e)
		{
			print($"파이어베이스 초기화 에러 : {e.Message}");
		}
	}

	private void AuthStateChanged(object sender, EventArgs e)
	{
		FirebaseAuth senderAuth = sender as FirebaseAuth;
		if (senderAuth != null)
		{
			_user = senderAuth.CurrentUser; //로그아웃일 때는 Null, 로그인일 때는 값이 있음
			if (_user != null)
			{
				_signInInfo.text = "User : " + _user.UserId;
			}
		}
	}



	//회원가입
	public async Task<bool> Create(string email, string password)
	{
		try
		{
			await _auth.CreateUserWithEmailAndPasswordAsync(email, password);

			return true;
		}
		catch (Exception e)
		{
			print($"계정 생성 에러 : {e.Message}");

			return false;
		}
	}

	//로그인
	public async Task<bool> SignIn(string email, string password)
	{
		try
		{
			await _auth.SignInWithEmailAndPasswordAsync(email, password);

			return true;
		}
		catch (Exception e)
		{
			print($"로그인 에러 : {e.Message}");

			return false;
		}
	}

	//로그아웃
	public void SignOut()
	{
		_auth.SignOut();
	}
}
