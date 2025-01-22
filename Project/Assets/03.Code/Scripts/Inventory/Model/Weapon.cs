public class Weapon : EquipItem
{

	public Weapon (WeaponData weaponData) : base(weaponData)
	{
		damage = weaponData.damage;
		defense = weaponData.defense;
		luck = weaponData.luck;
		health = weaponData.health;
	}
}
