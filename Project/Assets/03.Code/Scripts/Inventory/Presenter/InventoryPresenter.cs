using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
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
		
		inventory.OnEquipItem += GameManager.Instance.ItemDataManager.UpdateEquippedItem;
		inventory.OnUnequipItem += GameManager.Instance.ItemDataManager.UpdateUnequippedItem;
		
		// view to model
		inventoryPanel.OnRemoveItem += RemoveItem;
		inventoryPanel.OnEquipItem += inventory.EquipItem;
		inventoryPanel.OnUnequipItem += inventory.UnequipItem;
		
		AddItemCoroutine();
	}
	private void AddItemCoroutine()
	{
		Dictionary<int, Item> dic = GameManager.Instance.ItemDataManager.GetItemDictionary();
		print("인벤토리 초기 아이템 셋팅" + dic.Count);
		foreach (KeyValuePair<int, Item> itemPair in dic)
			inventory.Add(itemPair.Key, itemPair.Value,false);
		if (GameManager.Instance.ItemDataManager.equippedWeapon != -1)
			inventory.EquipItem(GameManager.Instance.ItemDataManager.equippedWeapon, true);
		if (GameManager.Instance.ItemDataManager.equippedArmor != -1)
			inventory.EquipItem(GameManager.Instance.ItemDataManager.equippedArmor, true);
		if (GameManager.Instance.ItemDataManager.equippedAccessory != -1)
			inventory.EquipItem(GameManager.Instance.ItemDataManager.equippedAccessory, true);
	}

	private void OnItemAdded(int index, Item item)
	{
		print("InventoryPresenter OnItemAdded");
		inventoryPanel.AddItem(index, item);
	}

	private void OnItemRemoved(int index, Item item)
	{
		inventoryPanel.RemoveItem(index);
	}

	public void RemoveItem(int slotNumber)
	{
		inventory.Remove(slotNumber);
	}
	
	public void UpdateEquippedItem(int slotNumber, EquipItem item)
	{
		playerStatusPanel.UpdateEquipItem(item);
	}
	
	public void UpdateUnequippedItem(EquipItem item)
	{
		playerStatusPanel.UpdateUnequipItem(item);
	}
	
	private void UpdateStatsFromItem(DataSnapshot itemRef, ref int atk, ref int def, ref int hp, ref int luck)
	{
		if (!itemRef.Exists)
			return;
		int itemId = int.Parse(itemRef.Value.ToString());
		if (itemId == -1)
			return;
		if (inventory.GetItem(itemId) is EquipItem item)
		{
			atk += item.damage;
			def += item.defense;
			hp += item.health;
			luck += item.luck;
		}
	}

	public async void UpdateStat()
	{
		DatabaseReference inventoryRef = GameManager.Instance.FirebaseManager.InventoryRef.Child(GameManager.Instance.FirebaseManager.User.UserId);
		DataSnapshot weaponRef = await inventoryRef.Child("equippedWeapon").GetValueAsync();
		DataSnapshot armorRef = await inventoryRef.Child("equippedArmor").GetValueAsync();
		DataSnapshot accessoryRef = await inventoryRef.Child("equippedAccessory").GetValueAsync();

		int atk = 0;
		int def = 0;
		int hp = 0;
		int luck = 0;

		UpdateStatsFromItem(weaponRef, ref atk, ref def, ref hp, ref luck);
		UpdateStatsFromItem(armorRef, ref atk, ref def, ref hp, ref luck);
		UpdateStatsFromItem(accessoryRef, ref atk, ref def, ref hp, ref luck);

		playerStatusPanel.attackText.text = $"- 공격력: {stat.atk} + {atk}";
		playerStatusPanel.defenseText.text = $"- 방어력: {stat.def} + {def}";
		playerStatusPanel.hpText.text = $"- 체력: {stat.hp} + {hp}";
		playerStatusPanel.luckText.text = $"- 운: {stat.luck} + {luck}";
	}
	
}
