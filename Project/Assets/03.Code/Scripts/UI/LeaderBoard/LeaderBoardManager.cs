using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
	[SerializeField] private LeaderBoard[] leaderBoards;
	private int currBoard = 0;
	
	public void OpenLeaderBoard()
	{
		currBoard = 0;
		gameObject.SetActive(true);
		leaderBoards[currBoard].gameObject.SetActive(true);
	}
	
	public void CloseLeaderBoard()
	{
		gameObject.SetActive(false);
	}

	public void NextLeaderBoard()
	{
		leaderBoards[currBoard].gameObject.SetActive(false);
		currBoard = (currBoard + 1) % leaderBoards.Length;
		leaderBoards[currBoard].gameObject.SetActive(true);
	}
}
