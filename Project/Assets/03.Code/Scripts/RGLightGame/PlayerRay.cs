using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
	public float distance;
	public LayerMask targetLayer;

	private void Update()
	{
		//if (Physics.Raycast(transform.position, transform.forward, out RaycastHit fhit, distance, targetLayer))
		//{
		//	float outDistance = distance - fhit.distance;
		//	print("앞 벗어난 길이 :" + outDistance);
		//}
		//if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit bhit, distance, targetLayer))
		//{
		//	float outDistance = distance - bhit.distance;
		//	print("뒤 벗어난 길이 :" + outDistance);
		//}
		//if (Physics.Raycast(transform.position, transform.right, out RaycastHit rhit, distance, targetLayer))
		//{
		//	float outDistance = distance - rhit.distance;
		//	print("오른쪽 벗어난 길이 :" + outDistance);
		//}
		//if (Physics.Raycast(transform.position, -transform.right, out RaycastHit lhit, distance, targetLayer))
		//{
		//	float outDistance = distance - lhit.distance;
		//	print("왼쪽 벗어난 길이 :" + outDistance);
		//}

	}
	public Vector3 CalcSpawnPoint()
	{
		Vector3 spawnPoint = transform.position;

		spawnPoint.x += GetOffset(-transform.right) - GetOffset(transform.right);
		spawnPoint.z += GetOffset(-transform.forward) - GetOffset(transform.forward);

		return spawnPoint;
	}

	private float GetOffset(Vector3 direction)
	{
		return Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, targetLayer)
			? distance - hit.distance
			: 0;
	}
}
