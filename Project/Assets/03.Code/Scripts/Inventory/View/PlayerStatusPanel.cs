using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusPanel : MonoBehaviour
{
	public Transform weaponSlot;
	public Transform armorSlot;
	public Transform accessorySlot;
	public Transform alcoholSlot;

	public TextMeshProUGUI attackText;
	public TextMeshProUGUI defenseText;
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI luckText;
	
	private Image _weaponIcon;
	private Image _armorIcon;
	private Image _accessoryIcon;
	private Image _alcoholIcon;

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
		else if (item is Alcohol alcoholItem)
		{
			if (_alcoholIcon != null)
				Destroy(_alcoholIcon.gameObject);
			_alcoholIcon = Instantiate(item.icon, alcoholSlot);
		}
	}
	
	public void UpdateUnequipItem(EquipItem item)
	{
		if (item is Weapon weaponItem && _weaponIcon != null)
			Destroy(_weaponIcon.gameObject);
		else if (item is Armor armorItem && _armorIcon != null)
			Destroy(_armorIcon.gameObject);
		else if (item is Accessory accessoryItem && _accessoryIcon != null)
			Destroy(_accessoryIcon.gameObject);
	}
}
