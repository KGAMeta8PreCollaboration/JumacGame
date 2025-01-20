public class Armor : EquipItem
{
	public int damage { get; private set; }
	public int defense { get; private set; }
	public int luck { get; private set; }
	public int health { get; private set; }
	
	public Armor(ArmorData weaponData) : base(weaponData)
	{
		defense = weaponData.defense;
	}
}
