public class Alcohol : EquipItem
{
    public Alcohol(AlcoholData data) : base(data)
    {
        damage = data.damage;
        defense = data.defense;
        luck = data.luck;
        health = data.health;
    }
}