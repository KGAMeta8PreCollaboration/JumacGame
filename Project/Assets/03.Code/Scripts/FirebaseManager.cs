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

public class FirebaseManager : Singleton<FirebaseManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    private DatabaseReference _logInUserRef;

    [SerializeField] private Text _signInInfo;

    private async void Start()
    {
        try
        {
            await FirebaseApp.CheckAndFixDependenciesAsync();

            Auth = FirebaseAuth.DefaultInstance;
            Database = FirebaseDatabase.DefaultInstance;

            //임시로 로그아웃 했음. 에디터에서 멈추고 해도 그 전 값이 있어서 그럼.
            //추후에 그냥 구글 연동이나 게스트 로그인은 로그인 화면을 거치지 않고 바로 갈꺼니 지우기만 하면 됌.
            SignOut();

            Auth.StateChanged += AuthStateChanged;

            print($"현재 사용자: {Auth.CurrentUser?.Email ?? "로그아웃 상태"}");
        }
        catch (Exception e)
        {
            print($"파이어베이스 초기화 에러 : {e.Message}");
        }
    }

    public async void FirebaseCheck()
    {
        try
        {
            DataSnapshot snapshot = await Database.GetReference("a/LoginUsers").GetValueAsync();

            if (snapshot.Exists)
            {
                print($"유저 수: {snapshot.ChildrenCount}");

                foreach (DataSnapshot data in snapshot.Children)
                {
                    if (data.HasChild("nickname"))
                    {
                        string nickname = data.Child("nickname").Value?.ToString() ?? "닉네임 없음";
                        print($"{data.Key}: {nickname}");
                    }
                    else
                    {
                        print($"{data.Key}: nickname 필드가 존재하지 않습니다.");
                    }
                }
            }
            else
            {
                print("해당 경로에 데이터가 없습니다.");
            }
        }
        catch (Exception e)
        {
            print($"데이터 읽기 에러: {e.Message}");
        }
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if (senderAuth != null)
        {
            User = senderAuth.CurrentUser; //로그아웃일 때는 Null, 로그인일 때는 값이 있음
            if (User != null)
            {
                _signInInfo.text = "User : " + User.UserId;
            }
        }
    }



    //회원가입
    public async Task<bool> Create(string email, string password)
    {
        try
        {
            AuthResult result = await Auth.CreateUserWithEmailAndPasswordAsync(email, password);

            LogInUserData userData = new LogInUserData(result.User.UserId);
            string json = JsonConvert.SerializeObject(userData);

            _logInUserRef = Database.GetReference("a").Child("LoginUsers");
            await _logInUserRef.Child(result.User.UserId).SetRawJsonValueAsync(json);
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
            AuthResult result = await Auth.SignInWithEmailAndPasswordAsync(email, password);

            string logInTime = DateTime.UtcNow.ToString("o");

            _logInUserRef = Database.GetReference("a").Child("LoginUsers");

            try
            {
                DataSnapshot snapshot = await _logInUserRef.Child(result.User.UserId).GetValueAsync();
                LogInUserData userData;
                if (snapshot.Exists)
                {
                    userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
                    userData.timestamp = logInTime;
                }
                else
                {
                    userData = new LogInUserData(result.User.UserId, logInTime);
                }

                string json = JsonConvert.SerializeObject(userData);
                await _logInUserRef.Child(result.User.UserId).SetRawJsonValueAsync(json);

                Debug.Log($"로그인 성공! 로그인 시간 업데이트: {logInTime}");
                return true;
            }
            catch (Exception e)
            {
                print($"데이터를 가져올 수 없음 : {e.Message}");

                //로그인은 성공했는데 데이터를 못가져 왔으니 로그아웃으로 로그인 못하게 해야 함.
                SignOut();
                return false;
            }
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
        Auth.SignOut();
    }

    public async Task<bool> SetNickname(string nickname)
    {
        try
        {
            DataSnapshot snapshot = await _logInUserRef.Child($"{User.UserId}").GetValueAsync();
            LogInUserData userData;

            if (snapshot.Exists)
            {
                userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
                userData.nickname = nickname;
            }
            else
            {
                userData = new LogInUserData(User.UserId, nickname: nickname);
            }

            string json = JsonConvert.SerializeObject(userData);
            await _logInUserRef.Child($"{User.UserId}").SetRawJsonValueAsync(json);
            Debug.Log($"닉네임 설정 성공! : {nickname}");
            return true;
        }
        catch (Exception e)
        {
            print($"닉네임 설정이 안됌 : {e.Message}");
            return false;
        }
    }

    public async Task<bool> SetKind(string kind)
    {
        try
        {
            DataSnapshot snapshot = await _logInUserRef.Child($"{User.UserId}").GetValueAsync();
            LogInUserData userData;

            if (snapshot.Exists)
            {
                userData = JsonConvert.DeserializeObject<LogInUserData>(snapshot.GetRawJsonValue());
                userData.kind = kind;
            }
            else
            {
                userData = new LogInUserData(User.UserId, kind: kind);
            }

            string json = JsonConvert.SerializeObject(userData);
            await _logInUserRef.Child($"{User.UserId}").SetRawJsonValueAsync(json);
            Debug.Log($"종족 설정 성공! : {kind}");
            return true;
        }
        catch (Exception e)
        {
            print($"종족 설정이 안됌 : {e.Message}");
            return false;
        }
    }
}
