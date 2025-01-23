using UnityEngine;

public class JoystickPresenter : MonoBehaviour
{
	private Joystick _joystick;
	private LocalPlayer _localPlayer;

	private void Start()
	{
		_joystick = FindObjectOfType<Joystick>(true);
		_localPlayer = FindObjectOfType<LocalPlayer>(true);
	}

	private void Update()
	{
		_localPlayer.moveDirection = new Vector3(-_joystick.dir.x, 0, -_joystick.dir.y);
	}
}
