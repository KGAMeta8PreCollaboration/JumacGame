using System;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerStatData
{
	public float atk;
	public float def;
	public float hp;
	public float luck;
	
	public float plusAtk;
	public float plusDef;
	public float plusHp;
	public float plusLuck;
	
	public PlayerStatData(Stat stat)
	{
		atk = stat.atk;
		def = stat.def;
		hp = stat.hp;
		luck = stat.luck;
		
		plusAtk = 0;
		plusDef = 0;
		plusHp = 0;
		plusLuck = 0;
	}

	public PlayerStatData()
	{
		atk = 10f;
		def = 5f;
		hp = 100f;
		luck = 2f;
		
		plusAtk = 0;
		plusDef = 0;
		plusHp = 0;
		plusLuck = 0;
	}
}

public class Stat : MonoBehaviour
{
	public float atk = 10f;
	public float def = 5f;
	public float hp = 100f;
	public float luck = 2f;
	
	public float plusAtk;
	public float plusDef;
	public float plusHp;
	public float plusLuck;

	private void Awake()
	{
		atk = 10f;
		def = 5f;
		hp = 100f;
		luck = 2f;
		
		plusAtk = 0;
		plusDef = 0;
		plusHp = 0;
		plusLuck = 0;
	}

	private void Start()
	{
		Init();
	}

	public async void Init()
	{
		DatabaseReference statRef = GameManager.Instance.FirebaseManager.StatRef;
		DataSnapshot statData = await statRef.Child(GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId).GetValueAsync();
		
		if (statData.Exists)
		{
			PlayerStatData data = JsonConvert.DeserializeObject<PlayerStatData>(statData.GetRawJsonValue());
			atk = data.atk;
			def = data.def;
			hp = data.hp;
			luck = data.luck;
			
			plusAtk = data.plusAtk;
			plusDef = data.plusDef;
			plusHp = data.plusHp;
			plusLuck = data.plusLuck;
		}
		else
		{
			string json = JsonConvert.SerializeObject(new PlayerStatData());
			await statRef.Child(GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
		}
		
		FindObjectOfType<InventoryPresenter>().UpdateStat();
	}
}
