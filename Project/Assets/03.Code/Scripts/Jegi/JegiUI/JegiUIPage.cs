using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JegiUIPage : JegiPage
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button restartButton;
    // Start is called before the first frame update

    private void Awake()
    {
        restartButton.onClick.AddListener(OnClickRestartButton);
    }

    public void SetScore(int scoreText)
    {
        this.scoreText.text = scoreText.ToString();
    }

    public void SetText(string resultText)
    {
        this.resultText.text = resultText;
    }

    private void OnClickRestartButton()
    {
        JegiGameManager.Instance.Restart();
    }
}
