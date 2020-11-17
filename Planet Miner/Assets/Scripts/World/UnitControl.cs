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
        unit.toggleSelected(true);
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
            unit.toggleSelected(false);

        _selectedUnits.Clear();
    }

    public void assignTaskToSelected(string task, GameObject target)
    {
        switch (task)
        {
            case "WalkTask":
                Vector3 targetPos = target.transform.position;
                foreach (Unit unit in _selectedUnits)
                {
                    Task t = TaskSystem.createWalkTask(targetPos);

                    unit.enqueueTask(t);

                    if (!t.multipleMode)
                        return;
                }

                break;

            case "DrillTask":
                Wall targetWall = target.GetComponent<Wall>();

                foreach (Unit unit in _selectedUnits)
                {
                    Task t = TaskSystem.createDrillTask(targetWall);

                    unit.enqueueTask(t);

                    if (!t.multipleMode)
                        return;
                }

                break;

            case "PickupTask":
                Resource resource = target.GetComponent<Resource>();
                foreach (Unit unit in _selectedUnits)
                {
                    Task t = TaskSystem.createPickupTask(resource);

                    unit.enqueueTask(t);

                    if (!t.multipleMode)
                        return;
                }
                break;

            case "DropTask":
                foreach (Unit unit in _selectedUnits)
                {
                    Task t = TaskSystem.createDropTask();

                    unit.enqueueTask(t);

                    if (!t.multipleMode)
                        return;
                }
                break;

            case "DigTask":
                Rubble rubble = target.GetComponent<Rubble>();
                foreach (Unit unit in _selectedUnits)
                {
                    Task t = TaskSystem.createDigTask(rubble);

                    unit.enqueueTask(t);

                    if (!t.multipleMode)
                        return;
                }
                break;
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
