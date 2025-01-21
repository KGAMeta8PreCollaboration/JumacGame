using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OmokPopup : MonoBehaviour
{
    protected Action<bool> closeAction;

    [SerializeField] private Button _closeButton;

    protected virtual void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseButtonClick);
    }

    protected virtual void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseButtonClick);
    }

    protected virtual void CloseButtonClick()
    {
        OmokUIManager.Instance.PopupClose();
        //closeAction?.Invoke();
    }
}
