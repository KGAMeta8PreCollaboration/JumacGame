using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFireEffect : MonoBehaviour
{
    public float damage;
    public int repeatCount;
    private Collider _coll;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
        StartCoroutine(RepeatCountCoroutine());
    }

    private IEnumerator RepeatCountCoroutine()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            yield return StartCoroutine(ColliderDisabled());
        }
    }
    private IEnumerator ColliderDisabled()
    {
        _coll.enabled = true;
        yield return new WaitForSeconds(0.1f);
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
