using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatPopup : LobbyPopup
{
    [SerializeField] private TMP_InputField inputChat;
    [SerializeField] private Button sendButton;
    [SerializeField] private RectTransform textArea;
    [SerializeField] private GameObject orangeChatPrefab;
    [SerializeField] private GameObject whiteChatPrefab;

    private void Awake()
    {
        sendButton.onClick.AddListener(OnClickSendButton);
    }

    private void Start()
    {
        LobbyFirebaseManager.Instance.ReceiveMessage(ReceiveMessage);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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

    private void ReceiveMessage(MessageData messageData)
    {
        string sender = LobbyFirebaseManager.Instance.chatUserData.id;
        GameObject messagePrefab = sender == messageData.SenderId
            ? whiteChatPrefab   //현재 플레이어
            : orangeChatPrefab; //다른 플레이어

        GameObject receivedMessageObj = Instantiate(messagePrefab, textArea);
        MessagePrefab receivedMessagePrefab = receivedMessageObj.GetComponent<MessagePrefab>();
        receivedMessagePrefab.messageText.text = $"{messageData.SenderName} : {messageData.Content}";
    }
}
