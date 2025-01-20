public abstract class EquipItem : Item
{
	public bool isEquipped { get; protected set; }
	
	public EquipItem (ItemData itemData) : base(itemData)
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
