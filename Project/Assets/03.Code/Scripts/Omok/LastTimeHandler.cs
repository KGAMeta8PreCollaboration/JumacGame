using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LastTimeHandler : Singleton<LastTimeHandler>
{
    [SerializeField] private TextMeshProUGUI rightTimeText; //내 타이머
    [SerializeField] private TextMeshProUGUI leftTimeText;  //상대 타이머

    private DatabaseReference _turnRef;

    private TimeSpan _turnDuration = TimeSpan.FromSeconds(30);
    private DateTime _turnEndTime;
    private bool _isOnGame;
    

    public void SetRef(DatabaseReference turnRef)
    {
        _turnRef = turnRef;
        _isOnGame = true;
    }

    private Coroutine _leftCo;
    private Coroutine _rightCo;

    public async void HandleTime()
    {
        //현재 누구 턴인지 알기위한 turnJson
        DataSnapshot turnRef = await _turnRef.Child("isHostTurn").GetValueAsync();
        bool isHostTurn = (turnRef.GetRawJsonValue() == "true");

        bool amIHost = OmokFirebaseManager.Instance.currentRoomData.host 
            == GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId;

        bool isMyTurn = ((amIHost && isHostTurn) || (!amIHost && !isHostTurn));

        //여기서 턴 시간 초기화
        _turnEndTime = DateTime.Now.Add(_turnDuration);

        if (isMyTurn && _isOnGame == true)
        {
            if (_leftCo != null) StopCoroutine(_leftCo);
            leftTimeText.text = $"남은 시간 : 30 : 00";

            if (_rightCo != null) StopCoroutine(_rightCo);
            _rightCo = StartCoroutine(HandleTimeCoroutine(rightTimeText, true));
        }

        else
        {
            if (_rightCo != null) StopCoroutine(_rightCo);
            rightTimeText.text = $"남은 시간 : 30 : 00";

            if (_leftCo != null) StopCoroutine(_leftCo);
            _leftCo = StartCoroutine(HandleTimeCoroutine(leftTimeText, false));
        }
    }

    public void IsOnGame(bool isOnGame)
    {
        _isOnGame = isOnGame;
        print($"onGame의 상태 : {_isOnGame}");
    }

    private IEnumerator HandleTimeCoroutine(TextMeshProUGUI timeText, bool isMyTimer)
    {
        OmokUserData myData =
                OmokFirebaseManager.Instance.amIHost ? OmokFirebaseManager.Instance.hostData : OmokFirebaseManager.Instance.guestData;
        while (_isOnGame)
        {
            TimeSpan lastTime = _turnEndTime - DateTime.Now;

            if (lastTime.TotalSeconds <= 0)
            {
                timeText.text = "남은 시간  : 00 : 00";
                bool amIWin = true;

                //여기에서 타임 초과되면 나올 UI띄우기
                if (isMyTimer)
                {
                    OmokFirebaseManager.Instance.UpdateUserResult(myData, !amIWin);
                    OmokOneButtonPopup popup = OmokUIManager.Instance.PopupOpen<OmokOneButtonPopup>();
                    popup.SetPopup(!amIWin, OmokFirebaseManager.Instance.currentRoomData.betting);
                }
                else
                {
                    OmokFirebaseManager.Instance.UpdateUserResult(myData, amIWin);
                    OmokOneButtonPopup popup = OmokUIManager.Instance.PopupOpen<OmokOneButtonPopup>();
                    popup.SetPopup(amIWin, OmokFirebaseManager.Instance.currentRoomData.betting);
                }
                _isOnGame = false;
                yield break;
            }

            timeText.text = $"남은 시간 : {lastTime.Minutes:D2} : {lastTime.Seconds:D2}";
            yield return null;
        }
    }
}
