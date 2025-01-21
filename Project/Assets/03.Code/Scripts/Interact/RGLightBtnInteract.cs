using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RGLightBtnInteract : ButtonInteractable
{
    public string nextScene;
    protected override void InteractionButtonClick()
    {
        InteractionInfoPopup infoPopup = PopupManager.Instance.PopupOpen("RGLightInfoPopup") as InteractionInfoPopup;
        infoPopup.onStartButtonClick = () => SceneManager.LoadScene(nextScene);
    }
}
