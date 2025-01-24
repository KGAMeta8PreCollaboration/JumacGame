using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JegiInteractable : ButtonInteractable
{

    private void Start()
    {
        buttonName = "제기차기 하러가기";
    }
    protected override void InteractionButtonClick()
    {
        InteractionInfoPopup infoPopup = PopupManager.Instance.PopupOpen("JegiInfoPopup") as InteractionInfoPopup;
        infoPopup.onStartButtonClick = () => SceneManager.LoadScene("JegiScene");
    }
}
