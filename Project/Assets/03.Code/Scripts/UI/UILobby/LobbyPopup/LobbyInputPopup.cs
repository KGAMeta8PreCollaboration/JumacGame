using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyInputPopup : LobbyPopup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button confirmButton;
    private Action<RoomData> _callback;

    protected override void OnEnable()
    {
        base.OnEnable();
        confirmButton.onClick.AddListener(OnClickConfirmButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        confirmButton.onClick.RemoveAllListeners();
    }

    public void SetPopup(string title, Action<RoomData> callback = null)
    {
        titleText.text = title;
        _callback = callback;
    }

    private void OnClickConfirmButton()
    {
        UILobbyManager.Instance.PopupClose();
        RoomData roomName = new RoomData(inputField.text);
        _callback?.Invoke(roomName);
    }
}
