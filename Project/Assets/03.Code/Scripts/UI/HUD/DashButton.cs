using UnityEngine;
using UnityEngine.EventSystems;

public class DashButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public LocalPlayer localPlayer;
	public float dashPower = 10f;
	public bool isHolding = false;
	
	private float _previousSpeed;

	private void Start()
	{
		localPlayer = FindObjectOfType<LocalPlayer>();
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if (localPlayer == null)
			localPlayer = FindObjectOfType<LocalPlayer>();
		if (localPlayer == null)
			return;
		_previousSpeed = localPlayer.speed;
		localPlayer.speed += dashPower;
		isHolding = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (localPlayer == null)
			localPlayer = FindObjectOfType<LocalPlayer>();
		if (localPlayer == null)
			return;
		localPlayer.speed = _previousSpeed;
		isHolding = false;
	}
}
