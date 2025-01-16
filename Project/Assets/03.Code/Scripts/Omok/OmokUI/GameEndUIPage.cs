using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUIPage : OmokPopup
{
    [SerializeField] private GameObject winImagePrefab;
    [SerializeField] private GameObject loseImagePrefab;
    [SerializeField] private RectTransform rectTransfrom;
    //닫는 버튼은 OmokPopup에서 cancelButton으로 대체

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void CloseButtonClick()
    {
        print("나가기 버튼 눌림");
        OmokFirebaseManager.Instance.ExitGame();
        base.CloseButtonClick();
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
