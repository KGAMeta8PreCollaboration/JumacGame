public class Accessory : EquipItem
{
	public int damage { get; private set; }
	public int defense { get; private set; }
	public int luck { get; private set; }
	public int health { get; private set; }
	
	public Accessory(AccessoryData weaponData) : base(weaponData)
	{
		luck = weaponData.luck;
	}
}
