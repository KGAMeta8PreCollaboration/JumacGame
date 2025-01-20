using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusPanel : MonoBehaviour
{
	public Transform weaponSlot;
	public Transform armorSlot;
	public Transform accessorySlot;
	
	private Image _weaponIcon;
	private Image _armorIcon;
	private Image _accessoryIcon;

	public void UpdateEquipItem(EquipItem item)
	{
		if (item is Weapon weaponItem)
		{
			if (_weaponIcon != null)
				Destroy(_weaponIcon.gameObject);
			_weaponIcon = Instantiate(item.icon, weaponSlot);
			
		}
		else if (item is Armor armorItem)
		{
			if (_armorIcon != null)
				Destroy(_armorIcon.gameObject);
			_armorIcon = Instantiate(item.icon, armorSlot);
		}
		else if (item is Accessory accessoryItem)
		{
			if (_accessoryIcon != null)
				Destroy(_accessoryIcon.gameObject);
			_accessoryIcon = Instantiate(item.icon, accessorySlot);
		}
	}
	
	public void UpdateUnequipItem(EquipItem item)
	{
		if (item is Weapon weaponItem)
		{
			Destroy(_weaponIcon.gameObject);
		}
		else if (item is Armor armorItem)
		{
			Destroy(_armorIcon.gameObject);
		}
		else if (item is Accessory accessoryItem)
		{
			Destroy(_accessoryIcon.gameObject);
		}
	}
}
