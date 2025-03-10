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

    private DateTime creationTime;

    private RoomData waitingRoom; //������ �ϳ��� ���� �� �־ List�� �ƴ� RoomData �����̴�.

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
        waitingRoom = roomData;

        titleText.text = roomData.roomName;

        creationTime = time;

        StartCoroutine(UpdateElapsedTimeCoroutine());
    }

    private IEnumerator UpdateElapsedTimeCoroutine()
    {
        while (true)
        {
            TimeSpan timePassed = DateTime.Now - creationTime;
            elapsedTimeText.text = $"{timePassed.Minutes:D2} : {timePassed.Seconds:D2}";

            yield return new WaitForSeconds(1f);
        }
    }

    protected override void CloseButtonClick()
    {
        base.CloseButtonClick();
        LobbyFirebaseManager.Instance.DeleteRoom(waitingRoom);
    }
}
