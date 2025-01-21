using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JegiUIPopup : JegiPopup
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void CloseButtonClick()
    {
        base.CloseButtonClick();
        Time.timeScale = 1.0f;
    }
}
