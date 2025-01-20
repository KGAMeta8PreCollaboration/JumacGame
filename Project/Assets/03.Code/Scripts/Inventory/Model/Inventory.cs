using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{ 
	public event Action<int, Item> OnItemAdded;
	public event Action<int, Item> OnItemRemoved;
	public event Action<EquipItem> OnEquipItem;
	public event Action<EquipItem> OnUnequipItem;
	public Weapon equippedWeapon { get; private set; }
	public Armor equippedArmor { get; private set; }
	public Accessory equippedAccessory { get; private set; }
	
	private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();
	

	public bool Add(int slotNumber, Item item)
	{
		if (itemDictionary.ContainsKey(slotNumber))
		{
			return false;
		}
		print("인벤토리 아이템 추가 액션 : " + OnItemAdded + "index = " + slotNumber);
		OnItemAdded?.Invoke(slotNumber, item);
		itemDictionary[slotNumber] = item;
		return true;
	}
	
	public bool Remove(Item item)
	{
		foreach (KeyValuePair<int, Item> itempair in itemDictionary)
		{
			if (itempair.Value == item)
			{ 
				OnItemRemoved?.Invoke(itempair.Key, item);
				itemDictionary.Remove(itempair.Key);
				return true;
			}
		}
		return false;
	}
	
	public bool Remove(int slotNumber)
	{
		if (itemDictionary.ContainsKey(slotNumber))
		{
			OnItemRemoved?.Invoke(slotNumber, itemDictionary[slotNumber]);
			itemDictionary.Remove(slotNumber);
			return true;
		}
		return false;
	}
	
	public void RemoveWithoutEvent(int slotNumber)
	{
		if (itemDictionary.ContainsKey(slotNumber))
		{
			itemDictionary.Remove(slotNumber);
		}
	}
	public Item GetItem(int slotNumber)
	{
		Item item = null;
		itemDictionary.TryGetValue(slotNumber, out item);
		return item;
	}
	
	public void EquipItem(int slotNumber)
	{
		Item item = GetItem(slotNumber);
		if (item is not EquipItem equipItem)
			return;
		if (item is Weapon)
		{
			EquipWeapon(item as Weapon);
		}
		else if (item is Armor)
		{
			EquipArmor(item as Armor);
		}
		else if (item is Accessory)
		{
			EquipAccessory(item as Accessory);
		}
		else
			return;
		OnEquipItem?.Invoke(equipItem);
	}
	
	public void EquipWeapon(Weapon weapon)
	{
		if (equippedWeapon != null)
		{
			equippedWeapon.Unequip();
		}
		if (itemDictionary.ContainsValue(weapon))
		{ 
			equippedWeapon = weapon;
			equippedWeapon.Equip();
		}
	}
	
	public void EquipArmor(Armor armor)
	{
		if (equippedArmor != null)
		{
			equippedArmor.Unequip();
		}
		if (itemDictionary.ContainsValue(armor))
		{
			equippedArmor = armor;
			equippedArmor.Equip();
		}
	}
	
	public void EquipAccessory(Accessory accessory)
	{
		if (equippedAccessory != null)
		{
			equippedAccessory.Unequip();
		}
		if (itemDictionary.ContainsValue(accessory))
		{
			equippedAccessory = accessory;
			equippedAccessory.Equip();
		}
	}
	
	
	public void UnequipItem(int slotNumber)
	{
		Item item = GetItem(slotNumber);
		if (item is EquipItem equipItem)
			equipItem.Unequip();
		else
			return;
		OnUnequipItem?.Invoke(equipItem);
	}
}
