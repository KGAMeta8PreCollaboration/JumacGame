using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JudgePopup : CombatPopup
{
    public TextMeshProUGUI judgeText;

    public void SetJudgeText(string text)
    {
        judgeText.text = text;
    }
}
