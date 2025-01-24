using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JegiInteractable : ButtonInteractable
{
    private void Start()
    {
        buttonName = "제기차러 가기";
    }
    protected override void InteractionButtonClick()
    {
        SceneManager.LoadScene("JegiScene");
    }
}
