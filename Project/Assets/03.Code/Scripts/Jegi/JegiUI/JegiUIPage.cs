using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JegiUIPage : JegiPage
{
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button pauseButton;
    // Start is called before the first frame update

    private void OnEnable()
    {
        restartButton.onClick.AddListener(OnClickRestartButton);
        pauseButton.onClick.AddListener(OnClickPauseButton);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
    }

    public void SetScore(int scoreText)
    {
        this.scoreText.text = scoreText.ToString();
    }

    public void SetCombo(int comboCount)
    {
        comboText.text = $"Combo {comboCount}";
    }

    private void OnClickRestartButton()
    {
        JegiGameManager.Instance.Restart();
    }

    private void OnClickPauseButton()
    {
        JegiUIManager.Instance.PopupOpen<JegiUIPopup>();
        Time.timeScale = 0f;
    }
}
