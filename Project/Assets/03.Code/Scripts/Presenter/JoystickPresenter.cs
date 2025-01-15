using UnityEngine;

public class JoystickPresenter : MonoBehaviour
{
	private Joystick _joystick;
	private LocalPlayer _localPlayer;

	private void Start()
	{
		_joystick = FindObjectOfType<Joystick>();
		_localPlayer = FindObjectOfType<LocalPlayer>();
	}

	private void Update()
	{
		_localPlayer.moveDirection = new Vector3(-_joystick.dir.x, 0, -_joystick.dir.y);
	}
}
