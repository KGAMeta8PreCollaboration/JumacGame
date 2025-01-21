using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JudgeTextPrefab : MonoBehaviour
{
    public TextMeshProUGUI judgeText;
    private Vector2 _targetPos;

    private void Awake()
    {
        _targetPos = transform.position + new Vector3(0, 3f, 0);
    }

    public void SetPrefab(string judgeText)
    {
        this.judgeText.text = judgeText;
        StartCoroutine(HandlePrefab());
    }

    private IEnumerator HandlePrefab()
    {
        Vector3 startPos = transform.position;
        float time = 0f;
        float duration = 1f;
        Color startColor = judgeText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(startPos, _targetPos, t);

            judgeText.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }
        Destroy(gameObject);
    }
}
