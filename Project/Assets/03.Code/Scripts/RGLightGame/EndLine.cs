using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{
	public class EndLine : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			//여기에 들어 왔다는 것. 골인했다는 것.
			RGLightManager.Instance.GameResult(true);
		}
	}
}
