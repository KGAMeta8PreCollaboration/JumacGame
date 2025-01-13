using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmokGameManager : Singleton<OmokGameManager>
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    public FirebaseUser User { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
    }

    public void SetUsers()
    {
        try
        {
            Auth = GameManager.Instance.FirebaseManager.Auth;
            Database = GameManager.Instance.FirebaseManager.Database;
            User = GameManager.Instance.FirebaseManager.User;

            string currentUserId = Auth.CurrentUser.UserId;
            print($"현재 유저의 Id : {currentUserId}");

            string hostId = OmokFirebaseManager.Instance.hostData.id;
            print($"호스트 Id : {hostId}");
            string guestId = OmokFirebaseManager.Instance.guestData.id;
            print($"게스트 Id : {guestId}");

            OmokUIManager.Instance.SetUserInfoPrefab(OmokFirebaseManager.Instance.hostData, OmokFirebaseManager.Instance.guestData);

            if (currentUserId == hostId)
            {
                print("현재 접속자는 호스트입니다");
            }
            else if (currentUserId == guestId)
            {
                print("현재 접속자는 게스트입니다");
            }
            else
            {
                print("호스트도, 게스트도 아닙니다");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase연결이 안됨 : {e.Message}");    
        }
    }

}
