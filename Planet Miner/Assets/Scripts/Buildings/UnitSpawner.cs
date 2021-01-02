using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : Building
{
    public Vector3 unitSpawn;

    private void Start()
    {
        unitSpawn = transform.position + transform.forward;
        unitSpawn.y = 1;
    }
}
