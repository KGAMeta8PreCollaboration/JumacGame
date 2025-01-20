using System.Collections.Generic;
using UnityEngine.UI;
public class Item
{
	public int id { get; protected set; }
	public string itemName { get; protected set; }
	public Image icon { get; protected set; }
	public string[] descriptions { get; protected set; }
	
	public Item(ItemData weaponData)
	{
		id = weaponData.id;
		icon = weaponData.icon.GetComponent<Image>();
		descriptions = weaponData.descriptions;
		itemName = weaponData.itemName;
		
	}
}
