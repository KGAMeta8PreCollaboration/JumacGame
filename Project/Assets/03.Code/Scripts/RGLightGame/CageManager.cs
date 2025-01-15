using Minigame.RGLight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageManager : MonoBehaviour
{
    [SerializeField] private GameObject _cagePrefab;
    public RGLightManager RGLightManager { get; private set; }

    public void Spawn(Transform spawnPoint)
    {
        Instantiate(_cagePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void Init(RGLightManager manager)
    {
        RGLightManager = manager;
    }
}
