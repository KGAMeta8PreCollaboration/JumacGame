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
    [SerializeField] private Button lobbyinbutton; //�̰� �浿�Բ�

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
    //    print($"Lobby�� �Ѿ�� �̸� : {userData.userName}");
    //}

    //public void SpawnPlayer(FirebaseUser fuser, UserData userData)
    //{
    //    Vector3 spawnPos = new Vector3(0, 1f, 0);
    //    GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

    //    Player player = playerObj.GetComponent<Player>();
    //    player.Init(fuser.UserId, userData.userName);
    //    playerDic[fuser.UserId] = player;
    //    print($"�÷��̾� ��ȯ�Ҷ� UserId : {fuser.UserId}, userName : {userData.userName}");
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
        ////���⿡�� ������ �ʴ����� ������
        //UIInput invitePopup = UIManager.Instance.PopupOpen<UIInput>();
        //invitePopup.InviteFiveEyes("�ʴ��ϱ�", (target) =>
        //InviteTarget(target)
        //    );
        UILobbyManager.Instance.PopupOpen<OmokPopup>();
    }

    //private void InviteTarget(string target)
    //{
    //    //���⿡ FirebaseManager���� �ʴ��ϴ� �Լ��� �־���� �ٵ� �̸��� �ִ��� �������� Ȯ��
    //}
}
