using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine.UI;
using Newtonsoft.Json;

public class FirebaseManager : MonoBehaviour
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private async void Start()
    {
        try
        {
            await FirebaseApp.CheckAndFixDependenciesAsync();

            Auth = FirebaseAuth.DefaultInstance;
            Database = FirebaseDatabase.DefaultInstance;

            //임시로 로그아웃 했음. 에디터에서 멈추고 해도 그 전 값이 있어서 그럼.
            //추후에 그냥 구글 연동이나 게스트 로그인은 로그인 화면을 거치지 않고 바로 갈꺼니 지우기만 하면 됌.
            GameManager.Instance.LogInManager.SignOut();

            Auth.StateChanged += AuthStateChanged;

            Debug.Log($"현재 사용자: {Auth.CurrentUser?.Email ?? "로그아웃 상태"}");
        }
        catch (Exception e)
        {
            Debug.LogError($"파이어베이스 초기화 에러 : {e.Message}");
        }
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if (senderAuth != null)
            User = senderAuth.CurrentUser;
    }
}
