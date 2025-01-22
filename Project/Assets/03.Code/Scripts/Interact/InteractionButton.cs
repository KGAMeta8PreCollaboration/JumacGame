using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    private TextMeshProUGUI _title;

    private void Awake()
    {
        _title = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTitle(string title)
    {
        _title.text = title;
    }
}
