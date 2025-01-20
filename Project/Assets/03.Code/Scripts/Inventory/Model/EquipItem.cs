public abstract class EquipItem : Item
{
	public bool isEquipped { get; protected set; }
	
	public EquipItem (ItemData weaponData) : base(weaponData)
	{
		isEquipped = false;
	}

	public void Equip()
	{
		isEquipped = true;
	}
	
	public void Unequip()
	{
		isEquipped = false;
	}
}
