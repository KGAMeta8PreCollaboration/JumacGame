using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TmpPlayerRotate : MonoBehaviour
{
	public float rotationSpeed = 5f;
	private Vector2 lookInput;

	public void OnLook(InputAction.CallbackContext context)
	{
		lookInput = context.ReadValue<Vector2>();
	}

	private void Update()
	{
		transform.eulerAngles = new Vector3(
			transform.eulerAngles.x + (lookInput.y * rotationSpeed * Time.deltaTime) ,
			transform.eulerAngles.y + (lookInput.x * rotationSpeed * Time.deltaTime), 0);
	}
}
