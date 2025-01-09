using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractionInfoPopup : Popup
{
    [SerializeField] private Button _startButton;
    public string nextScene;
    public string titleMessage;
    [SerializeField] private Text _title;
    [SerializeField] private SentenceData _sentenceDataPrefab;
    [SerializeField] private RectTransform _description;
    [SerializeField] private List<string> _sentenceList = new List<string>(6);

    private void Start()
    {
        _startButton.onClick.AddListener(StartButtonClick);

        _title.text = titleMessage;

        foreach (string sentence in _sentenceList)
        {
            SentenceData sentenceData = Instantiate<SentenceData>(_sentenceDataPrefab, _description);
            sentenceData.SetSentence(sentence);
        }
    }

    private void StartButtonClick()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(StartButtonClick);
    }
}
