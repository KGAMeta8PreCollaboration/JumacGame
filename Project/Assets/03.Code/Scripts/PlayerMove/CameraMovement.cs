using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
	public Transform playerTransform;
	public Transform objectTofollow;
	public float followSpeed = 10f;
	public float sensitivity = 100f;
	public float clampAngle = 80f;

	private float rotX;
	private float rotY;

	public Transform realCamera;
	public Vector3 dirNormalized;
	public Vector3 finalDir;
	
	public float minDistance = 2f;
	public float maxDistance = 10f;
	public float finalDistance = 5f;
	public float smoothness = 10f;

	public void OnLook(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
		rotX += input.y * sensitivity * Time.deltaTime;
		rotY += input.x * sensitivity * Time.deltaTime;
	}

	private void Start()
	{
		rotX = transform.eulerAngles.x;
		rotY = transform.eulerAngles.y;

		dirNormalized = realCamera.localPosition.normalized;
		finalDistance = realCamera.localPosition.magnitude;

		// Cursor.visible = false;
		// Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		// rotX += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
		// rotY += -Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
		
		playerTransform.rotation = Quaternion.Euler(0f, rotY, 0f);
		Quaternion rot = Quaternion.Euler(rotX, rotY, 0f);
		transform.rotation = rot;
	}

	private void LateUpdate()
	{
		transform.position = Vector3.MoveTowards(
			transform.position, objectTofollow.position, followSpeed * Time.deltaTime);

		finalDir = transform.TransformPoint(dirNormalized * maxDistance);
		RaycastHit hit;
		if (Physics.Linecast(objectTofollow.position, finalDir, out hit))
		{
			finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
		}
		else
		{
			finalDistance = maxDistance;
		}
		realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
	}
}
