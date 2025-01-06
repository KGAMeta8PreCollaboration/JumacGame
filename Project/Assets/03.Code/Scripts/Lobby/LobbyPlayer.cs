using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
	public string username;

	private Vector3 _position;
	public Vector3 position {
		get 
		{
			return _position;
		}
		set 
		{
			_position = value;
			transform.position = _position;
		}
	}
}
