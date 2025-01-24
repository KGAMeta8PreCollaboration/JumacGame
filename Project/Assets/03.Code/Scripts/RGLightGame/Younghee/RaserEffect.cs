using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaserEffect : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Minigame.RGLight.Player>(out Minigame.RGLight.Player player))
        {
            player.TakeDamage(damage);
        }
    }
}
