using UnityEngine.UI;
public class Item
{
	public int id { get; protected set; }
	public Image icon { get; protected set; }
	
	public Item(ItemData itemData)
	{
		id = itemData.id;
		icon = itemData.icon.GetComponent<Image>();
	}
}
