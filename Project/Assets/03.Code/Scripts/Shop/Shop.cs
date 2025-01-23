using Firebase.Database;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // 플레이어의 골드
    public int gold;
    
    private TextMeshProUGUI _goldText;

    private void Start()
    {
        _goldText = transform.Find("Gold").GetComponent<TextMeshProUGUI>();
        Init();
    }

    private async void Init()
    {
        DataSnapshot goldData = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}/gold").GetValueAsync(); 
        gold = int.Parse(goldData.Value.ToString());
        _goldText.text = "$ : " + gold;
    }


    public void SellItem(EquipItem item, int price)
    {
        if (gold < price)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }
        gold -= price;
        
        // 플레이어에게 아이템건네줌
    }
}
