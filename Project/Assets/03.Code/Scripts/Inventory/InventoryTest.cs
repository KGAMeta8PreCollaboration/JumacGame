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
		addWeaponButton.onClick.AddListener(AddWeaponUI);
	}
	
	public void AddWeaponUI()
	{
		Weapon weapon = new Weapon(weaponData);
		inventory.Add(0, weapon);
	}

}
