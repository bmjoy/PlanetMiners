using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    private List<Unit> _units;

    private List<Unit> _selectedUnits = new List<Unit>();

    public void addUnits(List<Unit> units)
    {
        _units = units;
    }

    public void selectSingleUnit(Unit unit)
    {
        unit.setSelected(true);
        _selectedUnits.Add(unit);
    }

    public void selectMultipleUnits(List<Unit> units)
    {

    }

    public void deselectUnits()
    {
        foreach (Unit unit in _selectedUnits)
            unit.setSelected(false);

        _selectedUnits.Clear();
    }

    public void assignTaskToSelected(string task, GameObject target)
    {
        switch (task)
        {
            case "WalkTask":
                Vector3 targetPos = target.transform.position;
                foreach (Unit unit in _selectedUnits)
                    unit.changeTask(new WalkTask(targetPos));
                break;

            case "DrillTask":
                Wall targetWall = target.GetComponent<Wall>();
                foreach (Unit unit in _selectedUnits)
                    unit.changeTask(new DrillTask(targetWall));
                break;

            case "PickupTask":
                Resource resource = target.GetComponent<Resource>();
                foreach (Unit unit in _selectedUnits)
                    unit.changeTask(new PickupTask(resource));
                break;

            case "DropTask":
                foreach (Unit unit in _selectedUnits)
                    unit.changeTask(new DropTask());
                break;
        }
    }
    public bool hasUnitsSelected()
    {
        return (_selectedUnits.Count > 0);
    }
}
