public class Weapon : EquipItem
{
	public int damage { get; private set; }
	public int defense { get; private set; }
	public int luck { get; private set; }
	public int health { get; private set; }

	public Weapon (WeaponData weaponData) : base(weaponData)
	{
		damage = weaponData.damage;
		defense = weaponData.defense;
		luck = weaponData.luck;
		health = weaponData.health;
	}
}
