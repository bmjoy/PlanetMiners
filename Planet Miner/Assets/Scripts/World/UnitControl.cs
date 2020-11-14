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
        foreach (Unit unit in units)
            selectSingleUnit(unit);
    }

    public void deselectUnits()
    {
        foreach (Unit unit in _selectedUnits)
            unit.setSelected(false);

        _selectedUnits.Clear();
    }

    public void assignTaskToSelected(string task, GameObject target)
    {
        Task taskToAssign = null;

        switch (task)
        {
            case "WalkTask":
                Vector3 targetPos = target.transform.position;
                taskToAssign = new WalkTask(targetPos);
                break;

            case "DrillTask":
                Wall targetWall = target.GetComponent<Wall>();
                taskToAssign = new DrillTask(targetWall);
                break;

            case "PickupTask":
                Resource resource = target.GetComponent<Resource>();
                taskToAssign = new PickupTask(resource);
                break;

            case "DropTask":
                taskToAssign = new DropTask();
                break;

            case "DigTask":
                Rubble rubble = target.GetComponent<Rubble>();
                taskToAssign = new DigTask(rubble, 1);
                break;
        }

        if (taskToAssign == null)
            return;

        foreach (Unit unit in _selectedUnits)
        {
            unit.changeTask(taskToAssign.clone());

            if (!taskToAssign.multipleMode)
                return;
        }
    }
    public bool hasUnitsSelected()
    {
        return (_selectedUnits.Count > 0);
    }

    public List<Unit> units()
    {
        return _units;
    }
}
