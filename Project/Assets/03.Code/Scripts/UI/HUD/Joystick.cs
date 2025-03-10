using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public enum FocusDirection
	{
		None,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	public Vector2 dir;
	
	[SerializeField] private GameObject[] focusObjects;
	[SerializeField] private RectTransform handle;
	[SerializeField] private float maxDistance = 150f;
	
	
	private Vector2 _startPosition;

	private void Reset()
	{
		maxDistance = 150f;
		handle = transform.Find("Handle").GetComponent<RectTransform>();
		Image[] objs = transform.Find("Focus").GetComponentsInChildren<Image>(true);
		focusObjects = new GameObject[objs.Length];
		for (int i = 0; i < objs.Length; i++)
			focusObjects[i] = objs[i].gameObject;
	}

	private void Awake()
	{
		_startPosition = handle.position;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		SetHandlePosition(eventData.position);
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		SetHandlePosition(_startPosition);
	}

	public void OnDrag(PointerEventData eventData)
	{
		SetHandlePosition(eventData.position);
	}
	
	private void SetHandlePosition(Vector2 position)
	{
		if (Vector2.Distance(_startPosition, position) > maxDistance)
			handle.position = _startPosition + (position - _startPosition).normalized * maxDistance;
		else
			handle.position = _startPosition + (position - _startPosition);
		
		dir = (_startPosition - (Vector2)handle.position).normalized;
		if (dir == Vector2.zero)
			SetFocusObjectsActive(FocusDirection.None);
		else if (dir.x > 0 && dir.y < 0)
			SetFocusObjectsActive(FocusDirection.TopLeft);
		else if (dir.x < 0 && dir.y < 0)
			SetFocusObjectsActive(FocusDirection.TopRight);
		else if (dir.x > 0 && dir.y > 0)
			SetFocusObjectsActive(FocusDirection.BottomLeft);
		else if (dir.x < 0 && dir.y > 0)
			SetFocusObjectsActive(FocusDirection.BottomRight);
	}
	
	private void SetFocusObjectsActive(FocusDirection direction)
	{
		foreach (GameObject obj in focusObjects)
			obj.SetActive(false);
		
		switch (direction)
		{
			case FocusDirection.TopLeft:
				focusObjects[0].SetActive(true);
				break;
			case FocusDirection.TopRight:
				focusObjects[1].SetActive(true);
				break;
			case FocusDirection.BottomLeft:
				focusObjects[2].SetActive(true);
				break;
			case FocusDirection.BottomRight:
				focusObjects[3].SetActive(true);
				break;
		}
		
	}
}
