using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWaveEffect : MonoBehaviour
{
    public float damage;
    private Collider _coll;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
        StartCoroutine(ColliderDisabled());
    }

    private IEnumerator ColliderDisabled()
    {
        _coll.enabled = true;
        yield return new WaitForSeconds(0.5f);
        _coll.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Minigame.RGLight.Player>(out Minigame.RGLight.Player player))
        {
            player.TakeDamage(damage);
        }
    }
}
