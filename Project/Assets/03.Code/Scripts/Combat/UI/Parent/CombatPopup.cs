using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatPopup : MonoBehaviour
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
        CombatUIManager.Instance.PopupClose();
        //closeAction?.Invoke();
    }
}
