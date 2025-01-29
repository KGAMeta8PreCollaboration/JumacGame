using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotPanel : MonoBehaviour, IPointerClickHandler
{
	public Item item { get; private set; }
	public ItemData itemData { get; private set; }

	private Shop _shop;
	private Image icon;
    
    
	public void SetItem(Item item, Shop shop)
	{
		_shop = shop;
		this.item = item;
		icon = Instantiate(item.icon, transform);
		
	}
	
	public void SetItem(ItemData itemData, Shop shop)
	{
		_shop = shop;
		this.itemData = itemData;
		icon = Instantiate(itemData.icon.GetComponent<Image>(), transform);
		item = itemData is WeaponData weaponData ? new Weapon(weaponData) :
			itemData is ArmorData armorData ? new Armor(armorData) :
			itemData is AccessoryData accessoryData ? new Accessory(accessoryData) :
			itemData is AlcoholData alcoholData ? new Alcohol(alcoholData) : null;
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		_shop.OpenShopSellItemPopup(item);
	}
}
