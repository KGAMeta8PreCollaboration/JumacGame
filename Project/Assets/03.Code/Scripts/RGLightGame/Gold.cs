using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.RGLight
{
    public class Gold : MonoBehaviour
    {
        public int moneyAmount;
        public float rotateSpeed;

        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Minigame.RGLight.Player>(out Minigame.RGLight.Player player))
            {
                player.RGLightManager.AddMoney(moneyAmount);
                AudioManager.Instance.PlaySfx(Sfx.GetCoin);
                Destroy(gameObject);
            }
        }
    }
}
