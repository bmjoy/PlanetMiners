using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrillTask : Task
{
    private Wall _targetWall;
    private Ground _groundNextToWall;

    public DrillTask(Wall wall)
    {
        _targetWall = wall;
        _multipleMode = true;
    }

    public override void start()
    {
        findWallGround();
        if (_groundNextToWall == null)
        {
            _taskEnded = true;
            return;
        }

        if (isNextToWall())
            unit.changeState(new Drilling(_targetWall, 1));
        else
        {
            if (Pathfinding.checkForPath(unit.transform.position, _groundNextToWall.transform.position))
                unit.insertTask(TaskSystem.createWalkTask(_groundNextToWall.transform.position));
            else
            {
                _taskEnded = true;
                return;
            }
        }
    }

    public override void execute()
    {
        if (isNextToWall() && unit.getState().GetType() != typeof(Drilling))
            unit.changeState(new Drilling(_targetWall, 1));
    }

    public override bool isFinished()
    {
        if (_targetWall == null || _taskEnded)
            return true;

        return false;
    }

    private void findWallGround()
    {
        GameObject nearestGround = null;
        float minDist = float.MaxValue;

        IEnumerable<GameObject> wallNeighbours =
            from neighbour in _targetWall.neighbours.Values
            where neighbour != null
            select neighbour;

        foreach (GameObject go in wallNeighbours)
        {
            if (go.TryGetComponent<Ground>(out Ground g))
                if (Mathf.Abs(Vector3.Distance(unit.transform.position, go.transform.position)) < minDist)
                {
                    nearestGround = go;
                    minDist = Mathf.Abs(Vector3.Distance(unit.transform.position, go.transform.position));
                }
        }

        if (nearestGround != null)
            _groundNextToWall = nearestGround.GetComponent<Ground>();
    }

    private bool isNextToWall()
    {
        return unit.isAtPosition(_groundNextToWall.transform.position);
    }

}


