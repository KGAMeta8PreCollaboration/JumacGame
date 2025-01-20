using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
	public Inventory inventory;
	public InventoryPanel inventoryPanel;
	public PlayerStatusPanel playerStatusPanel;
	
	public Stat stat;

	private void Start()
	{
		inventory = FindObjectOfType<Inventory>(true);
		inventoryPanel = FindObjectOfType<InventoryPanel>(true);
		playerStatusPanel = FindObjectOfType<PlayerStatusPanel>(true);
		stat = FindObjectOfType<Stat>(true);
		// model to view
		inventory.OnItemAdded += OnItemAdded;
		inventory.OnItemRemoved += OnItemRemoved;
		inventory.OnEquipItem += UpdateEquippedItem;
		inventory.OnUnequipItem += UpdateUnequippedItem;
		
		// view to model
		inventoryPanel.OnRemoveItem += RemoveItem;
		inventoryPanel.OnEquipItem += inventory.EquipItem;
		inventoryPanel.OnUnequipItem += inventory.UnequipItem;
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

	public void RemoveItem(int slotNumber)
	{
		inventory.Remove(slotNumber);
	}
	
	public void UpdateEquippedItem(EquipItem item)
	{
		playerStatusPanel.UpdateEquipItem(item);
	}
	
	public void UpdateUnequippedItem(EquipItem item)
	{
		playerStatusPanel.UpdateUnequipItem(item);
	}
	
	public void UpdateStat()
	{
		playerStatusPanel.attackText.text = $"- 공격력: {stat.atk}";
		playerStatusPanel.defenseText.text = $"- 방어력: {stat.def}";
		playerStatusPanel.hpText.text = $"- 체력: {stat.hp}";
		playerStatusPanel.luckText.text = $"- 운: {stat.luck}";
	}
	
}
