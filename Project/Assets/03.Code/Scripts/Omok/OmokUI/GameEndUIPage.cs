using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUIPage : OmokPopup
{
    [SerializeField] private GameObject winImagePrefab;
    [SerializeField] private GameObject loseImagePrefab;
    [SerializeField] private RectTransform rectTransfrom;
    [SerializeField] private Button retryButton;
    //닫는 버튼은 OmokPopup에서 cancelButton으로 대체

    protected override void OnEnable()
    {
        retryButton.onClick.AddListener(OnClickRetryButton);
    }

    protected override void OnDisable()
    {
        retryButton.onClick.RemoveAllListeners();
    }

    public void OnClickRetryButton()
    {

    }

    public void AmIWinner(bool isWin)
    {
        if (isWin == true)
        {
            Instantiate(winImagePrefab, rectTransfrom);
        }
        else
        {
            Instantiate(loseImagePrefab, rectTransfrom);
        }
    }
}
