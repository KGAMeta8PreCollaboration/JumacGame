using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPage : Page
{
    [SerializeField] private TextMeshProUGUI useridtext;
    [SerializeField] private Button gochatbutton;
    [SerializeField] private Button goomokbutton;
    [SerializeField] private Button lobbyinbutton; //이건 경동님꺼

    private void OnEnable()
    {
        gochatbutton.onClick.AddListener(OnClickGoChatButton);
        goomokbutton.onClick.AddListener(OnClickGoOmokButton);
    }

    private void OnDisable()
    {
        gochatbutton.onClick.RemoveAllListeners();
        goomokbutton.onClick.RemoveAllListeners();
    }

    //public void SetInfo(FirebaseUser fuser, UserData userData)
    //{
    //    userIdText.text = userData.userName;
    //    print($"Lobby에 넘어온 이름 : {userData.userName}");
    //}

    //public void SpawnPlayer(FirebaseUser fuser, UserData userData)
    //{
    //    Vector3 spawnPos = new Vector3(0, 1f, 0);
    //    GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

    //    Player player = playerObj.GetComponent<Player>();
    //    player.Init(fuser.UserId, userData.userName);
    //    playerDic[fuser.UserId] = player;
    //    print($"플레이어 소환할때 UserId : {fuser.UserId}, userName : {userData.userName}");
    //}

    //public void ShowMessageBubbleOnPlayer(string senderId, string message, MessageContentPrefab messageContentPrefab)
    //{
    //    Player senderPlayer = playerDic[senderId];
    //    Vector3 messageBubblePos = senderPlayer.transform.position + Vector3.up * 2f;
    //    MessageContentPrefab bubbleObj = Instantiate(messageContentPrefab, messageBubblePos, Quaternion.identity);
    //    bubbleObj.messageText.text = message;
    //}

    private void OnClickGoChatButton()
    {
        UILobbyManager.Instance.PopupOpen<ChatPopup>();
    }

    private void OnClickGoOmokButton()
    {
        ////여기에서 누구를 초대할지 떠야함
        //UIInput invitePopup = UIManager.Instance.PopupOpen<UIInput>();
        //invitePopup.InviteFiveEyes("초대하기", (target) =>
        //InviteTarget(target)
        //    );
        UILobbyManager.Instance.PopupOpen<OmokPopup>();
    }

    //private void InviteTarget(string target)
    //{
    //    //여기에 FirebaseManager에서 초대하는 함수를 넣어야함 근데 이름이 있는지 없는지도 확인
    //}
}
