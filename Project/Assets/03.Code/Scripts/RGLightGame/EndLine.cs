using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{
	public class EndLine : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			//���⿡ ��� �Դٴ� ��. �����ߴٴ� ��.
			RGLightManager.Instance.GameResult(true);
		}
	}
}
