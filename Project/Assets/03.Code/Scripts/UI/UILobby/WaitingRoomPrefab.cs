using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI hostInfoText;
    [SerializeField] private TextMeshProUGUI bettingText;
    [SerializeField] private Toggle roomToggle;
    [SerializeField] private Image backgroundImage;

    private ToggleGroup _toggleGroup;
    private bool _isSelected = false;
    private FindRoomPopup _findRoomPopup;
    private RoomData _roomData;

    private void Awake()
    {
        _isSelected = false;
        _toggleGroup = GetComponentInParent<ToggleGroup>();
        _findRoomPopup = FindObjectOfType<FindRoomPopup>();
        roomToggle.onValueChanged.AddListener(OnButtonClicked);

        if (_toggleGroup != null)
        {
            roomToggle.group = _toggleGroup;
        }
    }

    public async void SetInfo(RoomData roomData)
    {
        roomNameText.text = roomData.roomName;
        OmokUserData omokUserData = await SetUserData(roomData.host);
        print($"현재 유저의 이름 : {omokUserData.nickname}");
        print($"현재 방의 Key : {roomData.roomKey}");

        if (omokUserData != null)
        {
            if (omokUserData.win == 0 && omokUserData.lose == 0)
            {
                hostInfoText.text = $"{omokUserData.nickname}(승률 0%)";
                print("승리 조건이 없음");
            }
            else
            {
                float winPersent = ((float)omokUserData.win / (omokUserData.win + omokUserData.lose)) * 100;
                print($"승률 : {winPersent}");
                hostInfoText.text = $"{omokUserData.nickname}(승률 {winPersent:F2}%)";
                print("승리 조건 있음");
            }

            bettingText.text = roomData.betting.ToString();
            _roomData = roomData;
        }
    }

    private void OnButtonClicked(bool isOn)
    {
        _isSelected = !_isSelected;
        UpdateVisualState(_isSelected);
    }

    private void UpdateVisualState(bool isSelected)
    {
        if (isSelected)
        {
            backgroundImage.color = Color.gray;
            _findRoomPopup.SelectRoom(_roomData);
        }
        else
        {
            backgroundImage.color = Color.white;
        }
    }

    private async Task<OmokUserData> SetUserData(string id)
    {
        try
        {
            FirebaseDatabase Database = GameManager.Instance.FirebaseManager.Database;

            DatabaseReference omokDataRef = Database.GetReference("omokuserdata").Child(id);

            DataSnapshot omokDataSnapshot = await omokDataRef.GetValueAsync();

            //스냅샷이 존재하면 그대로 끌어다 쓰면 된다
            if (omokDataSnapshot.Exists)
            {
                string omokUserJson = omokDataSnapshot.GetRawJsonValue();
                OmokUserData omokUserData = JsonConvert.DeserializeObject<OmokUserData>(omokUserJson);

                return omokUserData;
            }
            //존재하지 않으면 UserId만 존재하는 OmokUSerData로 바꿔준다
            else
            {
                DatabaseReference lobbyUserData = Database.GetReference("loginusers").Child(id);
                DataSnapshot lobbyUserDataSnapshot = await lobbyUserData.GetValueAsync();

                string lobbyUserDataJson = lobbyUserDataSnapshot.GetRawJsonValue();
                LogInUserData logInUserData = JsonConvert.DeserializeObject<LogInUserData>(lobbyUserDataJson);

                OmokUserData newOmokUserData = new OmokUserData(id, logInUserData.nickname, logInUserData.gold);
                return newOmokUserData;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase 로그인 유저 참조 오류 : {e.Message}");
            return null;
        }
    }
}
