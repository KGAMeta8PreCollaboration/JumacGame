using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RGLightBtnInteract : ButtonInteractable
{
    public string nextScene;

    private void Start()
    {
        buttonName = "무궁화 게임 시작하기";
    }

    protected override void InteractionButtonClick()
    {
        InteractionInfoPopup infoPopup = PopupManager.Instance.PopupOpen("RGLightInfoPopup") as InteractionInfoPopup;
        infoPopup.onStartButtonClick = () => SceneManager.LoadScene(nextScene);
    }
}
