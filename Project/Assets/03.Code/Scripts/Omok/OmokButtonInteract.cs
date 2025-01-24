using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmokButtonInteract : ButtonInteractable
{
    private void Start()
    {
        buttonName = "오목하러 가기";
    }

    protected override void InteractionButtonClick()
    {
        UILobbyManager.Instance.PopupOpen<MakeOmokRoomPopup>();
    }
}
