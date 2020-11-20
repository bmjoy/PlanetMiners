using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : Building
{
    private Vector3 unitSpawn;

    private void Start()
    {
        unitSpawn = transform.position + transform.forward;
        unitSpawn.y = 1;
        terrainControl = FindObjectOfType<TerrainControl>();
    }
    public void spawnUnit()
    {
        terrainControl.spawnUnit(unitSpawn);
    }
}
