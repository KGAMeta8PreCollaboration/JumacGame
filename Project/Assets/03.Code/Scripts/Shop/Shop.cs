using System.Linq;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SlotPanel slotPanelPrefab;
    [SerializeField] private SellItemButton sellItemButton;
    [SerializeField] private Transform sellItemButtonParent;
    
    // 플레이어의 골드
    public int gold;
    
    private TextMeshProUGUI _goldText;

    private void Start()
    {
        _goldText = transform.Find("Gold").GetComponent<TextMeshProUGUI>();
        Init();
        PullGold();
    }

    private async void Init()
    {
        // DataSnapshot goldData = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}/gold").GetValueAsync(); 
        // gold = int.Parse(goldData.Value.ToString());
        // _goldText.text = "$ : " + gold;
        ItemData[] itemDatas = GameManager.Instance.ItemDataManager.itemDatas;

        itemDatas.Where(itemData => itemData.buyPrice != -1).ToList().ForEach(itemData =>
        {
            SellItemButton sellItemButton = Instantiate(this.sellItemButton, sellItemButtonParent);
            
            if (itemData is WeaponData weaponData)
                sellItemButton.SetItem(new Weapon(weaponData), weaponData.buyPrice);
            else if (itemData is ArmorData armorData)
                sellItemButton.SetItem(new Armor(armorData), armorData.buyPrice);
            else if (itemData is AccessoryData accessoryData)
                sellItemButton.SetItem(new Accessory(accessoryData), accessoryData.buyPrice);
            else if (itemData is AlcoholData alcoholData)
                sellItemButton.SetItem(new Alcohol(alcoholData), alcoholData.buyPrice);
        });
    }
    
    private async void PullGold()
    {
        DataSnapshot goldData = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child
            ($"{GameManager.Instance.FirebaseManager.User.UserId}/gold").GetValueAsync(); 
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
