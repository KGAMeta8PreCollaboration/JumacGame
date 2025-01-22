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

	public bool Add(Item item)
	{
		for (int i = 0; i < 20; i++)                         
		{
			if (itemDictionary.ContainsKey(i))
				continue;
			itemDictionary[i] = item;
			OnItemAdded?.Invoke(i, item);
			break;
		}
		GameManager.Instance.ItemDataManager.SaveItemDataToFirebase(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
		return true;
	}

	public bool Add(int slotNumber, Item item)
	{
		if (itemDictionary.ContainsKey(slotNumber))
		{
			return false;
		}
		itemDictionary[slotNumber] = item;
		OnItemAdded?.Invoke(slotNumber, item);
		GameManager.Instance.ItemDataManager.SaveItemDataToFirebase(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
		return true;
	}
	
	public bool Remove(Item item)
	{
		foreach (KeyValuePair<int, Item> itempair in itemDictionary)
		{
			if (itempair.Value == item)
			{ 
				OnItemRemoved?.Invoke(itempair.Key, item);
				GameManager.Instance.ItemDataManager.SaveItemDataToFirebase(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
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
			GameManager.Instance.ItemDataManager.SaveItemDataToFirebase(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
			return true;
		}
		return false;
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
			EquipWeapon(item as Weapon);
		else if (item is Armor)
			EquipArmor(item as Armor);
		else if (item is Accessory)
			EquipAccessory(item as Accessory);
		else
			return;
		OnEquipItem?.Invoke(equipItem);
	}
	
	public void EquipItem(EquipItem equipItem)
	{
		if (equipItem is Weapon) 
			EquipWeapon(equipItem as Weapon);
		else if (equipItem is Armor)
			EquipArmor(equipItem as Armor);
		else if (equipItem is Accessory)
			EquipAccessory(equipItem as Accessory);
		else
			return;
		OnEquipItem?.Invoke(equipItem);
	}
	
	public void EquipWeapon(Weapon weapon)
	{
		if (equippedWeapon != null)
			equippedWeapon.Unequip();
		if (itemDictionary.ContainsValue(weapon))
		{ 
			equippedWeapon = weapon;
			equippedWeapon.Equip();
		}
	}
	
	public void EquipArmor(Armor armor)
	{
		if (equippedArmor != null)
			equippedArmor.Unequip();
		if (itemDictionary.ContainsValue(armor))
		{
			equippedArmor = armor;
			equippedArmor.Equip();
		}
	}
	
	public void EquipAccessory(Accessory accessory)
	{
		if (equippedAccessory != null)
			equippedAccessory.Unequip();
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
