using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OmokPopup : LobbyPopup
{
    [SerializeField] private Button makeRoomButton;
    [SerializeField] private Button findRoomButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        makeRoomButton.onClick.AddListener(OnClickMakeRoomButton);
        findRoomButton.onClick.AddListener(OnClickFindRoomButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        makeRoomButton.onClick.RemoveAllListeners();
        findRoomButton.onClick.RemoveAllListeners();
    }

    private void OnClickMakeRoomButton()
    {
        //버튼을 누를때 Popup이 꺼지는 로직을 어떻게 할껀지 정해야한다.

    }

    private void OnClickFindRoomButton()
    {

    }
}
