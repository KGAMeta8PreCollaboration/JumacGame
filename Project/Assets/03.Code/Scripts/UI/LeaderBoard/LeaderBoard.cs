using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

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
	
	private DatabaseReference _omokRef;
	private DatabaseReference _loginusersRef;

	public string gameName = "omok";

	private void Start()
	{
		OmokRank();
	}
	
	private async void OmokRank()
	{
		try
		{
			await FirebaseApp.CheckAndFixDependenciesAsync();

			Auth = FirebaseAuth.DefaultInstance;
			Database = FirebaseDatabase.DefaultInstance;
			//추후에 그냥 구글 연동이나 게스트 로그인은 로그인 화면을 거치지 않고 바로 갈꺼니 지우기만 하면 됌.
			
			_omokRef = Database.GetReference($"leaderboard/{gameName}");
			_loginusersRef = Database.GetReference($"loginusers");
			
			DataSnapshot data = await _omokRef.GetValueAsync();
			int rank = 1;
			SortedDictionary<int, string> rankDict = new SortedDictionary<int, string>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
			foreach (DataSnapshot item in data.Children)
			{
				string nickname = item.Child("nickName").Value.ToString();
				int score = int.Parse(item.Child("win").Value.ToString()) * 10 - int.Parse(item.Child("lose").Value.ToString()) * 5;
				rankDict.Add(score, nickname);
			}
			
			foreach (KeyValuePair<int, string> kvp in rankDict)
			{
				LeaderBoardPanel panel = Instantiate(leaderBoardPanelPrefab, panelSpawnPoint);
				panel.SetData(kvp.Value, kvp.Key, rank++);
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"파이어베이스 초기화 에러 : {e.Message}");
		}
	}
}
