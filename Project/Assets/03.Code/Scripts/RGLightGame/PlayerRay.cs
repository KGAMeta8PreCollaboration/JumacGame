using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    public float distance;
    public LayerMask targetLayer;

    public bool forward;
    public bool backward;
    public bool left;
    public bool right;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit fhit, distance, targetLayer))
        {
            float outDistance = distance - fhit.distance;
            print("앞 벗어난 길이 :" + outDistance);
        }
        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit bhit, distance, targetLayer))
        {
            float outDistance = distance - bhit.distance;
            print("뒤 벗어난 길이 :" + outDistance);
        }

    }
}
