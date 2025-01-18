using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
	public Inventory inventory;
	public InventoryPanel inventoryPanel;
	
	private void Start()
	{
		inventory = FindObjectOfType<Inventory>(true);
		inventoryPanel = FindObjectOfType<InventoryPanel>(true);
		print(inventoryPanel);
		inventory.OnItemAdded += OnItemAdded;
		inventory.OnItemRemoved += OnItemRemoved;
	}
	
	private void OnItemAdded(int index, Item item)
	{
		inventoryPanel.AddItem(index, item);
	}
	
	private void OnItemRemoved(int index, Item item)
	{
		print("전달자 remove");
		inventoryPanel.RemoveItem(index);
	}
}
