using System;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public enum LeaderBoardType
{
	omok,
	rglight,
	jegi,
}

public class LeaderBoard : MonoBehaviour
{
	[SerializeField] private LeaderBoardPanel leaderBoardPanelPrefab;
	[SerializeField] private Transform panelSpawnPoint;

	public class OmokRankData
	{
		public int losecount;
		public int wincount;
	}
	
	public FirebaseAuth Auth { get; private set; }
	public FirebaseDatabase Database { get; private set; }
	
	private DatabaseReference _gameRankRef;
	private DatabaseReference _loginusersRef;

	public LeaderBoardType leaderBoardType;

	private void Start()
	{
		PullRank();
	}
	
	private async void PullRank()
	{
		try
		{
			await FirebaseApp.CheckAndFixDependenciesAsync();

			Auth = FirebaseAuth.DefaultInstance;
			Database = FirebaseDatabase.DefaultInstance;

			_gameRankRef = Database.GetReference($"leaderboard/{leaderBoardType.ToString()}");
			_loginusersRef = Database.GetReference($"loginusers");
		}
		catch (Exception e)
		{
			Debug.LogError($"파이어베이스 초기화 에러 : {e.Message}");
		}
		
		DataSnapshot data = await _gameRankRef.GetValueAsync();
		int rank = 1;
		List<KeyValuePair<int, string>> rankList = new List<KeyValuePair<int, string>>();

		foreach (DataSnapshot item in data.Children)
		{
			string nickname = item.Child("nickname").Value.ToString();
			int score = int.Parse(item.Child("score").Value.ToString());
			rankList.Add(new KeyValuePair<int, string>(score, nickname));
		}

		rankList.Sort((x, y) => y.Key.CompareTo(x.Key));

		foreach (KeyValuePair<int, string> kvp in rankList)
		{
			LeaderBoardPanel panel = Instantiate(leaderBoardPanelPrefab, panelSpawnPoint);
			panel.SetData(kvp.Value, kvp.Key, rank++);
		}
	}
}
