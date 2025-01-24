using Minigame.RGLight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    [SerializeField] private Gold _goldPrefab;
    public Vector3 spawnSize = new Vector3(29, 0, 29);

    public void Spawn(Vector3 cageCenter, int count)
    {
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPositionInCage(cageCenter);
            Instantiate(_goldPrefab, randomPosition, rotation);
        }
    }

    private Vector3 GetRandomPositionInCage(Vector3 center)
    {
        float randomX = Random.Range(center.x - spawnSize.x / 2, center.x + spawnSize.x / 2);
        float randomZ = Random.Range(center.z - spawnSize.z / 2, center.z + spawnSize.z / 2);
        float fixedY = 1f;

        return new Vector3(randomX, fixedY, randomZ);
    }
}
