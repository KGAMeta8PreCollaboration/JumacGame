using Minigame.RGLight;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerDistanceTracker : MonoBehaviour
{
	public float PlayerDistance
	{
		get
		{
			return System.MathF.Round(_playerDistance, 2);
		}
	}

	private Transform _startLine;
	private Transform _endLine;

	private float _totalUnityDistance;
	private float _playerDistance;

	private void Awake()
	{
		_startLine = GameObject.FindObjectOfType<StartLine>().transform;
		_endLine = GameObject.FindObjectOfType<EndLine>().transform;
	}

	private void Start()
	{
		_totalUnityDistance = _endLine.position.z - _startLine.position.z;
	}

	private void Update()
	{
		float playerUnityDistance = transform.position.z - _startLine.position.z;

		_playerDistance = Mathf.Clamp01(playerUnityDistance / _totalUnityDistance) * 150f;
	}
}
