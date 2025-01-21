public class Armor : EquipItem
{
	public int defense { get; private set; }
	
	public Armor(ArmorData itemData) : base(itemData)
	{
		defense = itemData.defense;
	}
}
