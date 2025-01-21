using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Minigame.RGLight
{
	public class PlayerInputManager : MonoBehaviour
	{
		public Vector2 InputMoveDir { get; private set; }

		public void OnMove(Context context)
		{
			InputMoveDir = context.ReadValue<Vector2>();
		}
	}
}
