using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{
    public class Money : MonoBehaviour
    {
        public int moneyAmount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Minigame.RGLight.Player>(out Minigame.RGLight.Player player))
            {
                player.RGLightManager.AddMoney(moneyAmount);
                Destroy(gameObject);
            }
        }
    }
}
