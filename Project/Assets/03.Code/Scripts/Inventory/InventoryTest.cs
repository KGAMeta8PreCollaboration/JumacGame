using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTest : MonoBehaviour
{ 
	public Inventory inventory;
	public WeaponData weaponData;
	public InventoryPanel inventoryPanel;
	public Button addWeaponButton;
	public Button removeWeaponButton;
	
	public TextMeshProUGUI addText;
	public TextMeshProUGUI removeText;
	

	private void Start()
	{
		// Weapon weapon = new Weapon(weaponData);
		// inventory.Add(0, weapon);
		//
		// print($"장착 시도");
		// inventory.EquipWeapon(weapon);
		
		addWeaponButton.onClick.AddListener(AddWeaponUI);
		removeWeaponButton.onClick.AddListener(RemoveWeaponUI);
	}
	
	public void AddWeaponUI()
	{
		Weapon weapon = new Weapon(weaponData);
		inventory.Add(0, weapon);
	}
	
	public void RemoveWeaponUI()
	{
		print("RemoveWeaponUI");
		inventory.Remove(0);
		// print("RemoveWeaponUI");
		// Item item = inventory.GetItem(0);
		// print(item);
		// inventoryPanel.RemoveItem(0);
	}
	
}
