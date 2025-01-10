using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingPopup : LobbyPopup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI elapsedTimeText;

    private DateTime creationTime;

    private RoomData waitingRoom; //대기방은 하나만 만들 수 있어서 List가 아닌 RoomData 형태이다.

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(UpdateElapsedTimeCoroutine());
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
        //여기에서 방 삭제하는 함수 추가
        ChatFirebaseManager.Instance.DeleteRoom(waitingRoom);
    }
}
