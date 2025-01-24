using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIPage : CombatPage
{
    [SerializeField] private Image leftHpBar;
    [SerializeField] private Image rightHpBar;
    [SerializeField] private TextMeshProUGUI leftUnitNameText;
    [SerializeField] private TextMeshProUGUI rightUnitNameText;

    private Coroutine _leftHpBarCoroutine = null;
    private Coroutine _rightHpBarCoroutine = null;

    public void SetPage(string leftUnitNameText, string rightUnitNameText)
    {
        this.leftUnitNameText.text = leftUnitNameText;
        this.rightUnitNameText.text = rightUnitNameText;

        leftHpBar.fillAmount = 1;
        rightHpBar.fillAmount = 1;

        _leftHpBarCoroutine = null;
        _rightHpBarCoroutine = null;
    }

    public void SetLeftHpBar(float hpAmount)
    {
        if (_leftHpBarCoroutine != null)
        {
            StopCoroutine( _leftHpBarCoroutine );
        }

        _leftHpBarCoroutine = StartCoroutine(LeftHpBarCoroutine(hpAmount));
    }

    public void SetRightHpBar(float hpAmount)
    {
        if (_rightHpBarCoroutine != null)
        {
            StopCoroutine(_rightHpBarCoroutine);
        }

        _rightHpBarCoroutine = StartCoroutine(RightHpBarCoroutine(hpAmount));
    }

    private IEnumerator LeftHpBarCoroutine(float targetFill)
    {
        float elapsedTime = 0;
        float duration = 1;
        float startFill = leftHpBar.fillAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            //0 ~ 1 사이값으로 보간
            float t = Mathf.Clamp01(elapsedTime / duration);

            leftHpBar.fillAmount = Mathf.Lerp(startFill, targetFill, t);

            yield return null;
        }

        //혹시모르니까 마지막엔 정확한 수치로 조정
        leftHpBar.fillAmount = targetFill;
        _leftHpBarCoroutine = null;
    }

    private IEnumerator RightHpBarCoroutine(float targetFill)
    {
        float elapsedTime = 0;
        float duration = 1;
        float startFill = rightHpBar.fillAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            //0 ~ 1 사이값으로 보간
            float t = Mathf.Clamp01(elapsedTime / duration);

            rightHpBar.fillAmount = Mathf.Lerp(startFill, targetFill, t);

            yield return null;
        }

        //혹시모르니까 마지막엔 정확한 수치로 조정
        rightHpBar.fillAmount = targetFill;
        _rightHpBarCoroutine = null;
    }
}
