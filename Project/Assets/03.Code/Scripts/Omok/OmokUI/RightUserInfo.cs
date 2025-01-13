using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RightUserInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private Image stoneImage;

    public void SetPrefab(string nickName, string record, bool isBlackStone)
    {
        this.nickNameText.text = nickName;
        this.recordText.text = record;
        if (isBlackStone)
        {
            stoneImage.color = Color.black;
        }
        else if (!isBlackStone)
        {
            stoneImage.color= Color.white;
        }
    }
}
