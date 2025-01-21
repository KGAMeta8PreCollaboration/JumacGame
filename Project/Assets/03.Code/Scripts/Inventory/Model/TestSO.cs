using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TestSO", order = 1)]
public class TestSO : ScriptableObject
{
	public int id;
	public string name;
	public int value;
	public Sprite icon;
}
