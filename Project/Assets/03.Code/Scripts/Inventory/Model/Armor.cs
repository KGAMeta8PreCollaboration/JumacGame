public class Armor : EquipItem
{
	public Armor(ArmorData data) : base(data)
	{
		damage = data.damage;
		defense = data.defense;
		luck = data.luck;
		health = data.health;
	}
}
