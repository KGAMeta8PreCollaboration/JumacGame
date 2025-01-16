using Minigame.RGLight;
using System.Collections.Generic;
using UnityEngine;


namespace Minigame.RGLight
{
    public class PlayerDistanceTracker : MonoBehaviour
    {
        [System.Serializable]
        public struct DistanceReward
        {
            public float maxDistance;
            public int score;
            public int money;
        }

        public List<DistanceReward> distanceRewards = new List<DistanceReward>();

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

        public int GetScore()
        {
            return GetReward().score;
        }

        public int GetMoney()
        {
            return GetReward().money;
        }

        private DistanceReward GetReward()
        {
            foreach (DistanceReward reward in distanceRewards)
            {
                if (PlayerDistance <= reward.maxDistance)
                    return reward;
            }
            return distanceRewards[distanceRewards.Count - 1];
        }
    }
}
