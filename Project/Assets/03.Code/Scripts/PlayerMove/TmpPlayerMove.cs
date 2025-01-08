using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TmpPlayerMove : MonoBehaviour
{
	public float speed = 5.0f;
	public float jumpPower = 5.0f;
	
	private Vector3 moveDirection = Vector3.zero;
	public Transform rotateTransform;
	public Rigidbody rb;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
		moveDirection = new Vector3(input.x, 0, input.y);
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
		}
	}

	private void LateUpdate()
	{ 
		Vector3 forwardMove = transform.forward * moveDirection.z * speed * Time.deltaTime;
		Vector3 rightMove = transform.forward * moveDirection.x * speed * Time.deltaTime;
		Vector3 move = forwardMove + rightMove;
		rb.MovePosition(transform.position - move);
	}
}
