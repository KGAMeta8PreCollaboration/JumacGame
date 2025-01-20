using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JegiUIPage : JegiPage
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    
    public void SetScore(int scoreText)
    {
        this.scoreText.text = scoreText.ToString();
    }

    public void SetText(string resultText)
    {
        this.resultText.text = resultText;
    }
}
