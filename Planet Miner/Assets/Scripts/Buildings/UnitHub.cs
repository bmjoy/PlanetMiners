using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHub : Building
{

    private List<Ore> _oresStored = new List<Ore>();
    private List<Crystal> _crystalsStored = new List<Crystal>();

    [SerializeField]
    private Transform dropPosition;

    public void addOre(Ore ore)
    {
        _oresStored.Add(ore);
        Destroy(ore.gameObject);
        EventManager.current.resourceChanged(0, _oresStored.Count);
    }

    public void removeOre()
    {
        if (_oresStored.Count > 0)
        {
            EventManager.current.spawnOre((int)dropPosition.position.x, (int)dropPosition.position.z);
            _oresStored.RemoveAt(0);
            EventManager.current.resourceChanged(0, _oresStored.Count);
        }
    }

    public void addCrystal(Crystal crystal)
    {
        _crystalsStored.Add(crystal);
        Destroy(crystal.gameObject);
        EventManager.current.resourceChanged(1, _crystalsStored.Count);
    }

    public void removeCrystal()
    {
        if (_crystalsStored.Count > 0)
        {
            EventManager.current.spawnCrystal((int)dropPosition.position.x, (int)dropPosition.position.z);
            _crystalsStored.RemoveAt(0);
            EventManager.current.resourceChanged(1, _crystalsStored.Count);
        }
    }

    public Tool getDrill()
    {
        return null;
    }

    public Tool getShovel()
    {
        return null;
    }
}
