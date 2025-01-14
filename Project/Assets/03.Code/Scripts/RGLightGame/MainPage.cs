using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.RGLight
{
	public class MainPage : Page
	{
		[SerializeField] private Text _remainTimeText;
		[SerializeField] private Text _moveDistanceText;

		public void SetRemainTime(string time)
		{
			_moveDistanceText.text = time;
		}

		public void SetMoveDistance(float distance)
		{
			_moveDistanceText.text = distance.ToString() + "m";
		}
	}
}
