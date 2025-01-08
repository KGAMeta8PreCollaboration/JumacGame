using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Popup : MonoBehaviour
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
        PopupManager.Instance.PopupClose();
        closeAction?.Invoke();
    }
}
