using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogToTMP : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform spawnPoint;
    private TextMeshProUGUI text;

    private List<TextMeshProUGUI> debugs = new();

    void Start()
    {
        Application.logMessageReceived += (condition, stackTrace, type) => 
        {
            TextMeshProUGUI a = Instantiate(textPrefab, spawnPoint).GetComponent<TextMeshProUGUI>();
            a.text = condition + "\n";
            debugs.Add(a);
        };
    }

    public void ClearDebugs()
    {
        foreach (var debug in debugs)
        {
            Destroy(debug.gameObject);
        }
        debugs.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
