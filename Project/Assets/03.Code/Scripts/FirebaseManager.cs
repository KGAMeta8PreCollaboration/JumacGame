using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System;
using System.Threading.Tasks;
using Firebase.Database;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseAuth _auth;
    private FirebaseDatabase _database;

    private async void Start()
    {
        try
        {
            await FirebaseApp.CheckAndFixDependenciesAsync();

            _auth = FirebaseAuth.DefaultInstance;
        }
        catch (Exception e)
        {
            print($"파이어베이스 초기화 에러 : {e.Message}");
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

    //로그아웃
}
