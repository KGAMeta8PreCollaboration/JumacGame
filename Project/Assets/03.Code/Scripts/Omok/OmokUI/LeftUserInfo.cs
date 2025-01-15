using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeftUserInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private Image stoneImage;

    public void SetPrefab(OmokUserData omokUserData, bool isHost)
    {
        this.nickNameText.text = omokUserData.nickname;
        this.recordText.text = $"승 : {omokUserData.win} 패 : {omokUserData.lose}";
        if (isHost)
        {
            stoneImage.color = Color.black;
        }
        else if (!isHost)
        {
            stoneImage.color= Color.white;
        }
    }
}
