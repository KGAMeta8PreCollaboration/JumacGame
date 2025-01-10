using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyPlayer : MonoBehaviour
{
	public string username;
	public string UID;

	private Vector3 _position;
	private Vector3 dir;
	
	public Vector3 position 
	{
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

	public void OnMove(InputAction.CallbackContext context)
	{
		print($"performed : {context.performed}");
		if (!context.performed)
		{
			dir = Vector3.zero;
			return;
		}
		
		Vector2 move = context.ReadValue<Vector2>();
		dir = new Vector3(move.x, 0, move.y);
	}
	

	private void Update()
	{
		position += dir * Time.deltaTime;
	}

}
