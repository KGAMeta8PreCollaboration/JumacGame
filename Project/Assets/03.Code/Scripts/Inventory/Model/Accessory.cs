public class Accessory : EquipItem
{
	
	public Accessory(AccessoryData data) : base(data)
	{
		damage = data.damage;
		defense = data.defense;
		luck = data.luck;
		health = data.health;
	}
}
