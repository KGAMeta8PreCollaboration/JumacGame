public class ItemInteract : ButtonInteractable
{
	public ItemData itemData;
	public Inventory inventory;

	private void Start()
	{
		buttonName = itemData.itemName + " 획득";
		inventory = FindObjectOfType<Inventory>(true);
	}

	protected override void InteractionButtonClick()
	{
		if (itemData is WeaponData weaponData)
		{
			Weapon weapon = new Weapon(weaponData);
			inventory.Add(weapon);
		}
		Destroy(gameObject);
	}
}
