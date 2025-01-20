public class Weapon : EquipItem
{
	public int damage { get; private set; }
	
	public Weapon (WeaponData itemData) : base(itemData)
	{
		damage = itemData.damage;
	}
}
