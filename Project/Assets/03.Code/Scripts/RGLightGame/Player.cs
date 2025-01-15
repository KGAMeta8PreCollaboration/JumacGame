using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{
	public class Player : MonoBehaviour
	{
		public float moveSpeed;
		public RGLightManager RGLightManager { get; private set; }

		public PlayerDistanceTracker PlayerDistanceTracker { get; private set; }
		public PlayerRay PlayerRay { get; private set; }
		private PlayerInputManager _playerInputManager;
		private Rigidbody _playerRigidbody;

		private void FixedUpdate()
		{
			if (RGLightManager.IsEndGame) return;
			Move(_playerInputManager.InputMoveDir);
		}

		private void Move(Vector2 input)
		{
			Vector3 actualMove = new Vector3(input.x, 0, input.y).normalized * moveSpeed * Time.fixedDeltaTime;
			_playerRigidbody.MovePosition(_playerRigidbody.position + actualMove);
		}

		public void Init(RGLightManager manager)
		{
			_playerInputManager = GetComponent<PlayerInputManager>();
			_playerRigidbody = GetComponent<Rigidbody>();
			PlayerDistanceTracker = GetComponent<PlayerDistanceTracker>();
			PlayerRay = GetComponent<PlayerRay>();
			RGLightManager = manager;
			CinemachineVirtualCamera cvc = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
			cvc.Follow = transform;
			cvc.LookAt = transform;
		}
	}
}
