using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIPage : LobbyPage
{
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private Button goChatButton;
    [SerializeField] private Button goOmokButton;
    [SerializeField] private Button lobbyInButton; //¿Ã∞« ∞Êµø¥‘≤®

    private void OnEnable()
    {
        goChatButton.onClick.AddListener(OnClickGoChatButton);
        goOmokButton.onClick.AddListener(OnClickGoOmokButton);
    }

    private void OnDisable()
    {
        goChatButton.onClick.RemoveAllListeners();
        goOmokButton.onClick.RemoveAllListeners();
    }

    public void SetUserInfo()
    {
        userNameText.text = ChatFirebaseManager.Instance.chatUserData.nickname;
    }

    private void OnClickGoChatButton()
    {
        UILobbyManager.Instance.PopupOpen<ChatPopup>();
    }

    private void OnClickGoOmokButton()
    {
        UILobbyManager.Instance.PopupOpen<OmokPopup>();
    }
}
