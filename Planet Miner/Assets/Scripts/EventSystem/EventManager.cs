using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action onSpawnUnit;
    public void spawnUnit()
    {
        if (onSpawnUnit != null)
            onSpawnUnit();
    }

    public event Action onDespawnUnit;

    public void deSpawnUnit()
    {
        if (onDespawnUnit != null)
            onDespawnUnit();
    }

    public event Action onUnitSpawned;

    public void unitSpawned()
    {
        if (onUnitSpawned != null)
            onUnitSpawned();
    }

    public event Action onUnitDespawned;

    public void unitDeSpawned()
    {
        if (onUnitDespawned != null)
            onUnitDespawned();
    }

    public event Action onUnitSelected;

    public void unitSelected()
    {
        if (onUnitSelected != null)
            onUnitSelected();
    }

    public event Action onUnitDeselected;

    public void unitDeSelected()
    {
        if (onUnitDeselected != null)
            onUnitDeselected();
    }

    public event Action onBuildingPlaced;

    public void buildingPlaced()
    {
        if (onBuildingPlaced != null)
            onBuildingPlaced();
    }

    public event Action onBuildingRemoved;

    public void buildingRemoved()
    {
        if (onBuildingRemoved != null)
            onBuildingRemoved();
    }
}
