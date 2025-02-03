using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatPopup : LobbyPopup
{
    [SerializeField] private TMP_InputField chatInputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private RectTransform textArea;
    [SerializeField] private MessagePrefab orangeChatPrefab;
    [SerializeField] private MessagePrefab whiteChatPrefab;

    private void Start()
    {
        LobbyFirebaseManager.Instance.ReceiveMessage(MakeTextPrefab);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        sendButton.onClick.AddListener(OnClickSendButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        sendButton.onClick.RemoveAllListeners();
    }


    private void OnClickSendButton()
    {
        ChatUserData chatUserData = LobbyFirebaseManager.Instance.chatUserData;

        MessageData message = new MessageData(
            chatUserData.id,
            chatUserData.nickname,
            chatUserData.servername,
            chatInputField.text
            );

        LobbyFirebaseManager.Instance.SendMessage(message);
        BubbleManager.Instance.MakeMyBubble(message);

        chatInputField.text = string.Empty;
    }

    private void MakeTextPrefab(MessageData messageData)
    {
        string sender = LobbyFirebaseManager.Instance.chatUserData.id;
        MessagePrefab messagePrefab = sender == messageData.SenderId
            ? whiteChatPrefab   
            : orangeChatPrefab;

        MessagePrefab receivedMessage = Instantiate(messagePrefab, textArea);
        Debug.Log($"새 메시지 부모: {receivedMessage.transform.parent.name}");
        receivedMessage.messageText.text = $"{messageData.SenderName} : {messageData.Content}";

        BubbleManager.Instance.MakeOtherBubble(messageData);
    }
}
