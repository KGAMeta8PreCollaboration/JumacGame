public class Accessory : EquipItem
{
	public int luck { get; private set; }
	public Accessory(AccessoryData itemData) : base(itemData)
	{
		luck = itemData.luck;
	}
}
