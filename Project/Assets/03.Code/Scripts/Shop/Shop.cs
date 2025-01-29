using System.Linq;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SlotPanel slotPanelPrefab;
    [SerializeField] private ShopSlotPanel shopSlotPanelPrefab;
    [SerializeField] private SellItemButton sellItemButton;
    [SerializeField] private Transform sellItemButtonParent;
    
    private ShopSellItemPopup _shopSellItemPopup;
    private Inventory _inventory;
    
    // 플레이어의 골드
    public int gold { get; private set; }
    
    private TextMeshProUGUI _goldText;

    private void Start()
    {
        _shopSellItemPopup = FindObjectOfType<ShopSellItemPopup>(true);
        _shopSellItemPopup.SetShop(this);
        _goldText = transform.Find("Gold").GetComponent<TextMeshProUGUI>();
        _inventory = FindObjectOfType<Inventory>(true);
        Init();
        PullGold();
        CloseShop();
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
    }
    
    public void CloseShop()
    {
        gameObject.SetActive(false);
    }

    private async void Init()
    {
        ItemData[] itemDatas = GameManager.Instance.ItemDataManager.itemDatas;

        itemDatas.Where(itemData => itemData.buyPrice != -1).ToList().ForEach(itemData =>
        {
            ShopSlotPanel shopSlotPanel = Instantiate(shopSlotPanelPrefab, sellItemButtonParent);
            shopSlotPanel.SetItem(itemData, this);
        });
        GameManager.Instance.FirebaseManager.LogInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}/gold").ValueChanged += GoldChanged;
    }
    private void GoldChanged(object sender, ValueChangedEventArgs e)
    {
        PullGold();
    }

    private async void PullGold()
    {
        DataSnapshot goldData = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child
            ($"{GameManager.Instance.FirebaseManager.User.UserId}/gold").GetValueAsync(); 
        gold = int.Parse(goldData.Value.ToString());
        _goldText.text = "엽전 : " + gold;
    }

    public bool SellItem(Item item, int price)
    {
        print($"판매중 플레이어 골드 {gold}, 아이템 가격 {price}");
        if (gold < price)
        {
            Debug.Log("골드가 부족합니다.");
            return false;
        }
        GameManager.Instance.FirebaseManager.LogInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}/gold")
            .SetValueAsync(gold - price);
        gold -= price;
        _inventory.Add(item);
        return true;
    }
    
    public void OpenShopSellItemPopup(Item item)
    {
        _shopSellItemPopup.OpenPopup();
        _shopSellItemPopup.SetItem(item);
    }
}
