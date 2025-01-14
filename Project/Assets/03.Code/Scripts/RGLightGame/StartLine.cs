using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
	private Collider _coll;

	private void Awake()
	{
		_coll = GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<Minigame.RGLight.Player>(out Minigame.RGLight.Player player))
		{
			_coll.enabled = false;
			StartCoroutine(player.RGLightManager.TimeCheckCoroutine());
			StartCoroutine(player.RGLightManager.MainPageUpdateCoroutine());
		}
	}
}
