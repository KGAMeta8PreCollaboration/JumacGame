using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
	[Header("모든 ItemData SO를 넣어주세요.")]
	[SerializeField] private ItemData[] itemDatas;
		
	public Dictionary<int, int> itemDictionary = new Dictionary<int, int>();
	private Inventory _inventory;

	private void Start()
	{
		StartCoroutine(LoadItemData());
	}

	public IEnumerator LoadItemData()
	{
		yield return new WaitUntil(() => GameManager.Instance.FirebaseManager.User != null);
		LoadItemDataToFirebase();
	}

	private async void LoadItemDataToFirebase()
	{
		DatabaseReference inventoryRef = GameManager.Instance.FirebaseManager.InventoryRef;

		string uid = GameManager.Instance.FirebaseManager.User.UserId;
		if ((await inventoryRef.GetValueAsync()).Exists)
		{
			DataSnapshot data = await inventoryRef.Child(uid).GetValueAsync();

			InventoryData inventoryData = new InventoryData();
			inventoryData.equippedWeapon = Convert.ToInt32(data.Child("equippedWeapon").GetValue(true));
			inventoryData.equippedArmor = Convert.ToInt32(data.Child("equippedArmor").GetValue(true));
			inventoryData.equippedAccessory = Convert.ToInt32(data.Child("equippedAccessory").GetValue(true));
			foreach (DataSnapshot item in data.Child("itemDictionary").Children)
				inventoryData.itemDictionary[int.Parse(item.Key)] = Convert.ToInt32(item.Value);
			itemDictionary = inventoryData.itemDictionary;
			print("파이어베이스에서 아이템 정보 로드 완료");
		}
		else
		{
			InventoryData inventoryData = new InventoryData(itemDictionary, null, null, null);
			string json = JsonConvert.SerializeObject(inventoryData);
			await inventoryRef.Child(uid).SetRawJsonValueAsync(json);
		}
	}

	public Dictionary<int, Item> GetItemDictionary()
	{ 
		Dictionary<int, Item> resDic = new Dictionary<int, Item>();
		Dictionary<int, int> tmp = new Dictionary<int, int>(itemDictionary);
		
		foreach (KeyValuePair<int, int> itemPair in tmp)
		{
			for (int i = 0; i < itemDatas.Length; i++)
			{
				if (itemDatas[i].id == itemPair.Value)
				{
					Item item = new Item(itemDatas[i]);
					resDic.Add(itemPair.Key, item);
					break;
				}
			}
		}
		return resDic;
	}
	
	public async void SaveItemDataToFirebase(Dictionary<int, Item> itemDic, Weapon equippedWeapon, Armor equippedArmor, Accessory equippedAccessory)
	{
		itemDictionary.Clear();
		foreach (KeyValuePair<int, Item> item in itemDic)
			itemDictionary[item.Key] = item.Value.id;
		InventoryData inventoryData = new InventoryData(itemDictionary, equippedWeapon, equippedArmor, equippedAccessory);
		DatabaseReference inventoryRef = GameManager.Instance.FirebaseManager.InventoryRef;
		string uid = GameManager.Instance.FirebaseManager.User.UserId;
		string json = JsonConvert.SerializeObject(inventoryData);
		await inventoryRef.Child(uid).SetRawJsonValueAsync(json);
	}
}

public class InventoryData
{
	public Dictionary<int, int> itemDictionary = new Dictionary<int, int>();
	public int equippedWeapon;
	public int equippedArmor;
	public int equippedAccessory;

	public InventoryData()
	{
		equippedWeapon = -1;
		equippedArmor = -1;
		equippedAccessory = -1;
	}
	
	public InventoryData(Dictionary<int, int> itemDic, Weapon equippedWeapon, Armor equippedArmor, Accessory equippedAccessory)
	{
		itemDictionary = new Dictionary<int, int>(itemDic);
		if (equippedWeapon == null)
			this.equippedWeapon = -1;
		else
			this.equippedWeapon = equippedWeapon.id;
		if (equippedArmor == null)
			this.equippedArmor = -1;
		else
			this.equippedArmor = equippedArmor.id;
		if (equippedAccessory == null)
			this.equippedAccessory = -1;
		else
			this.equippedAccessory = equippedAccessory.id;
	}
}
