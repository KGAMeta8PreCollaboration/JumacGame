using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JegiResultPopup : JegiPopup
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI goldText;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void SetPopup(int score, int combo, int gold)
    {
        scoreText.text = $"점수 : {score}";
        comboText.text = $"횟수 : {combo}";
        goldText.text = $"X {gold}";
    }

    protected override void CloseButtonClick()
    {
        base.CloseButtonClick();
    }
}
