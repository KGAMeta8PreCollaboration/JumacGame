using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{
	public class Player : MonoBehaviour
	{
		public float moveSpeed;

		private PlayerInputManager _playerInputManager;
		private Rigidbody _playerRigidbody;

		private void Awake()
		{
			_playerInputManager = GetComponent<PlayerInputManager>();
			_playerRigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			Move(_playerInputManager.InputMoveDir);
		}

		private void Move(Vector2 input)
		{
			Vector3 actualMove = new Vector3(input.x, 0, input.y).normalized * moveSpeed * Time.fixedDeltaTime;
			_playerRigidbody.MovePosition(_playerRigidbody.position + actualMove);
		}
	}
}
