using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeftUserInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI totalGoldText;
    [SerializeField] private Image stoneImage;

    private OmokUserData _omokUserData;

    public void SetPrefab(OmokUserData omokUserData, bool isHost)
    {
        _omokUserData = omokUserData;
        this.nickNameText.text = _omokUserData.nickname;
        this.recordText.text = $"승 : {_omokUserData.win} 패 : {_omokUserData.lose}";
        totalGoldText.text = $"{omokUserData.gold}";

        if (isHost)
        {
            stoneImage.color = Color.black;
        }
        else if (isHost == false)
        {
            stoneImage.color= Color.white;
        }
        MonitorResult(recordText);
    }

    //실시간으로 omokuserdata가 바뀌면 승,패 text와 골드량이 보인다
    private void MonitorResult(TextMeshProUGUI recordText)
    {
        FirebaseDatabase omokdata = GameManager.Instance.FirebaseManager.Database;

        DatabaseReference omokDataRef = omokdata.GetReference("omokuserdata").Child(_omokUserData.id);

        //omokuserdata의 값이 바뀔 때 마다 실행됨 -> 즉 게임이 끝나고 나서
        omokDataRef.ValueChanged += async (sender, args) =>
        {
            if (args.Snapshot.Exists)
            {
                DataSnapshot omokDataSnapshot = await omokDataRef.GetValueAsync();

                string omokDataJson = omokDataSnapshot.GetRawJsonValue();
                OmokUserData omokUserData = JsonConvert.DeserializeObject<OmokUserData>(omokDataJson);

                recordText.text = $"승 : {omokUserData.win} 패 : {omokUserData.lose}";
                totalGoldText.text = $"{omokUserData.gold}";
            }
            else
            {
                Debug.LogWarning("omokUserData의 스냅샷이 존재하지 않음");
                return;
            }
        };
    }
}
