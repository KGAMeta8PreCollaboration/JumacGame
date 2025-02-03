using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPopup : LobbyPopup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI elapsedTimeText;
    [SerializeField] private Button popupCloseButton;

    private DateTime _creationTime;

    private RoomData _waitingRoom; //������ �ϳ��� ���� �� �־ List�� �ƴ� RoomData �����̴�.

    protected override void OnEnable()
    {
        base.OnEnable();
        popupCloseButton.onClick.AddListener(CloseButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(UpdateElapsedTimeCoroutine());
        popupCloseButton.onClick.RemoveListener(CloseButtonClick) ;
    }

    public void SetWaitingRoom(RoomData roomData, DateTime time)
    {
        _waitingRoom = roomData;

        titleText.text = roomData.roomName;

        _creationTime = time;

        StartCoroutine(UpdateElapsedTimeCoroutine());
    }

    private IEnumerator UpdateElapsedTimeCoroutine()
    {
        while (true)
        {
            TimeSpan timePassed = DateTime.Now - _creationTime;
            elapsedTimeText.text = $"{timePassed.Minutes:D2} : {timePassed.Seconds:D2}";

            yield return new WaitForSeconds(1f);
        }
    }

    protected override void CloseButtonClick()
    {
        base.CloseButtonClick();
        LobbyFirebaseManager.Instance.DeleteRoom(_waitingRoom);
    }
}
