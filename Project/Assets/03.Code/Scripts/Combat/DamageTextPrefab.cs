using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextPrefab : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    [SerializeField] private Vector3 targetPos;

    private void Start()
    {
        targetPos = transform.up * 1;
    }

    public void SetDamageText(float damage)
    {
        damageText.text = damage.ToString();

        StartCoroutine(HandlePrefab());
    }

    private IEnumerator HandlePrefab()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + targetPos;

        Color startColor = damageText.color;
        float startAlpha = startColor.a;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            Color newColor = damageText.color;
            newColor.a = Mathf.Lerp(startAlpha, 0f, t);
            damageText.color = newColor;
            yield return null;
        }

        Destroy(gameObject);
    }
}
