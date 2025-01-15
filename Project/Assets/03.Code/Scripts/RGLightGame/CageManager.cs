using Minigame.RGLight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageManager : MonoBehaviour
{
    [SerializeField] private GameObject _cagePrefab;
    public RGLightManager RGLightManager { get; private set; }

    private GameObject _cage;

    public void Spawn(Transform spawnPoint)
    {
        _cage = Instantiate(_cagePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void DestroyCage()
    {
        Destroy(_cage);
    }

    public void Init(RGLightManager manager)
    {
        RGLightManager = manager;
    }
}
