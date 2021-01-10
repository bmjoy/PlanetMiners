using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : Building
{
    [SerializeField]
    private Transform spawnTransform;
    public Vector3 unitSpawn;

    private void Start()
    {
        unitSpawn = spawnTransform.position;   
    }
}
