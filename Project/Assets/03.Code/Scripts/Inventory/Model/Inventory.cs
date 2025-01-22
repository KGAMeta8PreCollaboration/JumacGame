using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{ 
	public event Action<int, Item> OnItemAdded;
	public event Action<int, Item> OnItemRemoved;
	public event Action<int, EquipItem> OnEquipItem;
	public event Action<EquipItem> OnUnequipItem;
	public Weapon equippedWeapon { get; private set; }
	public Armor equippedArmor { get; private set; }
	public Accessory equippedAccessory { get; private set; }
	
	private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

	public bool Add(Item item, bool isAction = true)
	{
		for (int i = 0; i < 20; i++)                         
		{
			if (itemDictionary.ContainsKey(i))
				continue;
			itemDictionary[i] = item;
			OnItemAdded?.Invoke(i, item);
			break;
		}
		if (isAction)
			GameManager.Instance.ItemDataManager.SaveItemDataToFirebase(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
		return true;
	}

	public bool Add(int slotNumber, Item item, bool isAction = true)
	{
		if (itemDictionary.ContainsKey(slotNumber))
		{
			return false;
		}
		itemDictionary[slotNumber] = item;
		OnItemAdded?.Invoke(slotNumber, item);
		if (isAction)
			GameManager.Instance.ItemDataManager.SaveItemDataToFirebase(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
		return true;
	}
	
	public bool Remove(Item item, bool isAction = true)
	{
		foreach (KeyValuePair<int, Item> itempair in itemDictionary)
		{
			if (itempair.Value == item)
			{ 
				OnItemRemoved?.Invoke(itempair.Key, item);
				itemDictionary.Remove(itempair.Key);
				if (isAction)
					GameManager.Instance.ItemDataManager.SaveItemDataToFirebase(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
				return true;
			}
		}
		return false;
	}
	
	public bool Remove(int slotNumber, bool isAction = true)
	{
		if (itemDictionary.ContainsKey(slotNumber))
		{
			OnItemRemoved?.Invoke(slotNumber, itemDictionary[slotNumber]);
			itemDictionary.Remove(slotNumber);
			if (isAction)
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
	
	public void EquipItem(int slotNumber, bool isAction = true)
	{
		print(itemDictionary.Count);
		Item item = GetItem(slotNumber);
		if (item is not EquipItem equipItem)
			return;
		print("아이템이 장착됩니다.");
		if (item is Weapon weapon)
			EquipWeapon(weapon);
		else if (item is Armor armor)
			EquipArmor(armor);
		else if (item is Accessory accessory)
			EquipAccessory(accessory);
		else
			return;
		if (isAction)
			OnEquipItem?.Invoke(slotNumber, equipItem);
	}
	
	public void EquipWeapon(Weapon weapon)
	{
		print("무기가 장착됩니다.");
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
