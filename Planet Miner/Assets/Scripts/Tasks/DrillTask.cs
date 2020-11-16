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
        if (!isNextToWall())
        {
            if (Pathfinding.checkForPath(unit.transform.position, _groundNextToWall.transform.position))
                unit.changeState(new Walking(unit.transform.position, _groundNextToWall.transform.position, unit));
            else
                end();
        }

        else
            unit.changeState(new Drilling(_targetWall, 1, unit));
    }

    public override void execute()
    {
        if (taskEnded) return;

        if (isNextToWall() && unit.getState().GetType() != typeof(Drilling))
            unit.changeState(new Drilling(_targetWall, 1, unit));
    }

    public override bool isFinished()
    {
        return (_targetWall == null);
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


        if (_groundNextToWall == null)
            end();
    }

    private bool isNextToWall()
    {
        findWallGround();
        return unit.isAtPosition(_groundNextToWall.transform.position);
    }
}


