using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JegiPopup : MonoBehaviour
{
    protected Action callback;

    [SerializeField] private Button closeButton;

    protected virtual void OnEnable()
    {
        closeButton.onClick.AddListener(CloseButtonClick);
    }

    protected virtual void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseButtonClick);
    }

    protected virtual void CloseButtonClick()
    {
        JegiUIManager.Instance.PopupClose();
    }
}
