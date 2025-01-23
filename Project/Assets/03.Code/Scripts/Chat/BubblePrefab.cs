using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubblePrefab : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetText(string text)
    {
        this.text.text = text;
        StartCoroutine(HandleBubble());
    }

    private IEnumerator HandleBubble()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
