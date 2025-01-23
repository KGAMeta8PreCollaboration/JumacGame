using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ItemData
{
	public int damage;
	public int defense;
	public int luck;
	public int health;
}
