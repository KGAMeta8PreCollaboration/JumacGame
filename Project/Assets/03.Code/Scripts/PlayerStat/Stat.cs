using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class Stat : MonoBehaviour
{
	public float atk;
	public float def;
	public float hp;
	public float luck;

	private void Reset()
	{
		atk = 10f;
		def = 5f;
		hp = 100f;
		luck = 2f;
	}

	private async void Start()
	{
		StartCoroutine(Test());
	}

	private IEnumerator Test()
	{
		yield return new WaitForSeconds(1f);
		Init();
	}

	public async void Init()
	{
		DatabaseReference a= GameManager.Instance.FirebaseManager.StatRef;
		DataSnapshot b = await a.GetValueAsync();


		StartCoroutine(CoroutineTest());

		// print(b.Child(GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId));
		// a.SetRawJsonValueAsync();
		// key : GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId
		// value : Stat
		
	}
	
	private IEnumerator CoroutineTest()
	{
		while (GameManager.Instance.FirebaseManager.Auth.CurrentUser == null)
		{
			yield return new WaitForSeconds(1f);
			print(GameManager.Instance.FirebaseManager.Auth);
		}
		print(GameManager.Instance.FirebaseManager.Auth.CurrentUser);
		print(GameManager.Instance.FirebaseManager.Auth.CurrentUser.UserId);
	}

}
