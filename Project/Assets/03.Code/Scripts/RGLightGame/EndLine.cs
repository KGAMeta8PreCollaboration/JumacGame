using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{
	public class EndLine : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Minigame.RGLight.Player>(out Minigame.RGLight.Player player))
			{
				player.RGLightManager.GameResult(true);
			}
		}
	}
}
