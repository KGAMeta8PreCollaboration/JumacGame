using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardBtnInteract : ButtonInteractable
{
	public LeaderBoardManager LeaderBoardManager;

	private void Start()
	{
		LeaderBoardManager = FindObjectOfType<LeaderBoardManager>(true);
		buttonName = "랭킹 보기";
	}
	protected override void InteractionButtonClick()
	{
		LeaderBoardManager.OpenLeaderBoard();
		// UILobbyManager.Instance.PopupOpen<LeaderBoardPopup>();
	}
}
