public abstract class EquipItem : Item
{
	public bool isEquipped { get; protected set; }
	public int damage { get; protected set; }
	public int defense { get; protected set; }
	public int luck { get; protected set; }
	public int health { get; protected set; }
	
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
