using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIPage : LobbyPage
{
    [SerializeField] private Button goChatButton;

    private void OnEnable()
    {
        goChatButton.onClick.AddListener(OnClickGoChatButton);
        //LobbyFirebaseManager.Instance.ReceiveMessage(SubscribeBubble);
    }

    private void OnDisable()
    {
        goChatButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        LobbyFirebaseManager.Instance.ReceiveMessage(SubscribeBubble);
    }

    private void OnClickGoChatButton()
    {
        UILobbyManager.Instance.PopupOpen<ChatPopup>();
    }

    private void SubscribeBubble(MessageData messageData)
    {
        BubbleManager.Instance.MakeOtherBubble(messageData);
    }
}
