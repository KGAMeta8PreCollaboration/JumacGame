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
            print($"���� ������ Id : {currentUserId}");

            string hostId = OmokFirebaseManager.Instance.hostData.id;
            print($"ȣ��Ʈ Id : {hostId}");
            string guestId = OmokFirebaseManager.Instance.guestData.id;
            print($"�Խ�Ʈ Id : {guestId}");

            

            if (currentUserId == hostId)
            {
                print("���� �����ڴ� ȣ��Ʈ�Դϴ�");
            }
            else if (currentUserId == guestId)
            {
                print("���� �����ڴ� �Խ�Ʈ�Դϴ�");
            }
            else
            {
                print("ȣ��Ʈ��, �Խ�Ʈ�� �ƴմϴ�");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase������ �ȵ� : {e.Message}");    
        }
    }

}
