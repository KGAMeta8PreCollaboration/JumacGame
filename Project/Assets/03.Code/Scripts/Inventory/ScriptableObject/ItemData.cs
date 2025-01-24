using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public abstract class ItemData : ScriptableObject
{
	public int id;
	public int buyPrice;
	public string itemName;
	public GameObject icon;
	public string[] descriptions;
}
