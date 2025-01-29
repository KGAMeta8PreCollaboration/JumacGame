public class Weapon : EquipItem
{

	public Weapon (WeaponData itemData) : base(itemData)
	{
		damage = itemData.damage;
		defense = itemData.defense;
		luck = itemData.luck;
		health = itemData.health;
	}
}
