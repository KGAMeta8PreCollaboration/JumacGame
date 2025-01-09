using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopup : MonoBehaviour
{
    protected Action closeAction;

    [SerializeField] private Button _closeButton;

    protected virtual void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseButtonClick);
    }

    protected virtual void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseButtonClick);
    }

    private void CloseButtonClick()
    {
        UILobbyManager.Instance.PopupClose();
        closeAction?.Invoke();
    }
}
