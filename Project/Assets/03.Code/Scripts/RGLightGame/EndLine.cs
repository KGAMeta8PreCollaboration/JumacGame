using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{

    public class EndLine : MonoBehaviour
    {
        private Collider _coll;

        private void Awake()
        {
            _coll = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Minigame.RGLight.Player>(out Minigame.RGLight.Player player))
            {
                _coll.enabled = false;
                player.RGLightManager.GameResult(true);
            }
        }
    }
}
