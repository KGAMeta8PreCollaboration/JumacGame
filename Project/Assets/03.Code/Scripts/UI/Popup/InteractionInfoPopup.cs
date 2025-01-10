using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractionInfoPopup : Popup
{
    [SerializeField] private Button _startButton;
    [SerializeField] private SentenceData _sentenceDataPrefab;
    public string nextScene;
    public string titleMessage;
    [SerializeField] private List<string> _sentenceList = new List<string>(6);

    private Text _title;
    private RectTransform _description;

    private void Awake()
    {
        _title = transform.Find("InteractionInfoPanel/MainTitle").GetComponent<Text>();
        _description = transform.Find("InteractionInfoPanel/DescriptionPanel/Description").GetComponent<RectTransform>();
    }

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
