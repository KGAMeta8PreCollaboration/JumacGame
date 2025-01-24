using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellItemButton : MonoBehaviour
{
	private Image _itemImage;
	private TextMeshProUGUI _itemNameText;
	private TextMeshProUGUI _itemAtkText;
	private TextMeshProUGUI _itemDefText;
	private TextMeshProUGUI _itemHpText;
	private TextMeshProUGUI _itemLuckText;
	private TextMeshProUGUI _itemPriceText;
	
	private Button _sellButton;
	private Shop _shop;
	
	private void Awake()
	{
		_itemImage = transform.Find("Image").GetComponent<Image>();
		_itemNameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		_itemAtkText = transform.Find("Atk").GetComponent<TextMeshProUGUI>();
		_itemDefText = transform.Find("Def").GetComponent<TextMeshProUGUI>();
		_itemHpText = transform.Find("Hp").GetComponent<TextMeshProUGUI>();
		_itemLuckText = transform.Find("Luck").GetComponent<TextMeshProUGUI>();
		_itemPriceText = transform.Find("Price").GetComponent<TextMeshProUGUI>();
		_sellButton = GetComponent<Button>();
		_sellButton.onClick.AddListener(Sell);
	}
	
	public void SetItem(EquipItem item, int sellGold)
	{
		_itemImage.sprite = item.icon.sprite;
		_itemNameText.text = item.itemName;
		_itemAtkText.text = item.damage.ToString();
		_itemDefText.text = item.defense.ToString();
		_itemHpText.text = item.health.ToString();
		_itemLuckText.text = item.luck.ToString();
		_itemPriceText.text = sellGold.ToString();
	}

	public void Sell()
	{
		
	}
}
