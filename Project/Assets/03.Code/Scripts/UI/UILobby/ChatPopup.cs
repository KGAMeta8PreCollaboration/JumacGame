using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatPopup : Popup
{
    [SerializeField] private TMP_InputField inputChat;
    [SerializeField] private Button sendButton;

    private void Awake()
    {
        sendButton.onClick.AddListener(OnClickSendButton);
    }

    private void OnClickSendButton()
    {
        ChatUserData chatUserData = LobbyFirebaseManager.Instance.chatUserData;

        MessageData message = new MessageData(
            chatUserData.id,
            chatUserData.nickname,
            chatUserData.serverName,
            inputChat.text
            );

        LobbyFirebaseManager.Instance.SendMessage(message);
    }
}
