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
    #region unitEvents
    public event Action onSpawnUnit;
    public void spawnUnit()
    {
        if (onSpawnUnit != null)
            onSpawnUnit();
    }

    public event Action onDeSpawnUnit;

    public void deSpawnUnit()
    {
        if (onDeSpawnUnit != null)
            onDeSpawnUnit();
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

    #endregion

    #region buildingEvents

    public event Action<string> onShowGhostBuilding;

    public void showGhostBuilding(string buildingName)
    {
        if (onShowGhostBuilding != null)
            onShowGhostBuilding(buildingName);
    }


    public event Action onPlacingBuilding;

    public void placingBuilding()
    {
        if (onPlacingBuilding != null)
            onPlacingBuilding();
    }

    public event Action onBuildingPlaced;

    public void buildingPlaced()
    {
        if (onBuildingPlaced != null)
            onBuildingPlaced();
    }

    public event Action<Vector3> onRemoveBuilding;

    public void removeBuilding(Vector3 position)
    {
        if (onRemoveBuilding != null)
            onRemoveBuilding(position);
    }

    public event Action onCancelPlaceBuilding;

    public void cancelPlaceBuilding()
    {
        if (onCancelPlaceBuilding != null)
            onCancelPlaceBuilding();
    }

    public event Action <int,int>onResourceChanged;

    public void resourceChanged(int index,int value)
    {
        onResourceChanged?.Invoke(index,value);
    }

    #endregion

    #region resourceEvents
    public event Action <int, int>onSpawnOre;

    public void spawnOre(int x, int y)
    {
        onSpawnOre?.Invoke(x, y);
    }

    public event Action <int,int> onSpawnCrystal;

    public void spawnCrystal(int x, int y)
    {
        onSpawnCrystal?.Invoke(x,y);
    }

    #endregion
}
