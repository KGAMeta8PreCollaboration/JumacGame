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
    }

    private void OnDisable()
    {
        goChatButton.onClick.RemoveAllListeners();
    }

    private void OnClickGoChatButton()
    {
        UILobbyManager.Instance.PopupOpen<ChatPopup>();
    }
}
