using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JegiUIPage : JegiPage
{
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button pauseButton;
    // Start is called before the first frame update

    private void OnEnable()
    {
        restartButton.onClick.AddListener(OnClickRestartButton);
        pauseButton.onClick.AddListener(OnClickPauseButton);
        startButton.onClick.AddListener(OnClickStartButton);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveAllListeners();
    }

    public void SetScore(int currentScore, int bestScore)
    {
        this.currentScoreText.text = currentScore.ToString();

        if (bestScore >= currentScore)
        {
            this.bestScoreText.text = bestScore.ToString();
        }
        else 
        {
            this.bestScoreText.text = currentScore.ToString();
            JegiFirebaseManager.Instance.jegiUserData.score = currentScore;
        }
    }

    public void SetCombo(int comboCount)
    {
        comboText.text = $"Combo {comboCount}";
    }

    private void OnClickStartButton()
    {
        JegiGameManager.Instance._pause = false;
        startButton.gameObject.SetActive(false);
        //JegiUIManager.Instance.PopupOpen<JegiTimePopup>().StartTimer();
    }

    public void SetStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    private void OnClickRestartButton()
    {
        JegiGameManager.Instance.Restart();
    }

    private void OnClickPauseButton()
    {
        JegiGameManager.Instance._pause = true;
        JegiUIManager.Instance.PopupOpen<JegiUIPopup>();
        Time.timeScale = 0f;
    }
}
