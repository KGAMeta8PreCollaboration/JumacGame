using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
	public float distance;
	public LayerMask targetLayer;

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
