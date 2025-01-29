using UnityEngine.UI;
public class Item
{
	public int id { get; protected set; }
	public string itemName { get; protected set; }
	public Image icon { get; protected set; }
	public string[] descriptions { get; protected set; }
	public int buyPrice { get; protected set; }

	
	public Item(ItemData itemData)
	{
		id = itemData.id;
		icon = itemData.icon.GetComponent<Image>();
		descriptions = itemData.descriptions;
		itemName = itemData.itemName;
		buyPrice = itemData.buyPrice;
	}

	public Item()
	{
		id = 0;
		itemName = "Empty";
		icon = null;
		descriptions = new string[0];
	}
}
