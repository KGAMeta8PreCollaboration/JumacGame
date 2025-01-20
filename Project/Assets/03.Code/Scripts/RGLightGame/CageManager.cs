using Minigame.RGLight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageManager : MonoBehaviour
{
    [SerializeField] private GameObject _cagePrefab;
    public RGLightManager RGLightManager { get; private set; }

    public Cage cage { get; private set; }

    public void Spawn(Vector3 spawnPoint)
    {
        cage = Instantiate(_cagePrefab, spawnPoint, Quaternion.identity).GetComponent<Cage>();
    }

    public void DestroyCage()
    {
        Destroy(cage.gameObject);
    }

    public void Init(RGLightManager manager)
    {
        RGLightManager = manager;
    }
}
