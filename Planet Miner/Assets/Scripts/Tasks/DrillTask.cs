using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillTask : Task
{
    private Wall _targetWall;
    private Ground _groundNextToWall;
    public DrillTask(Wall wall)
    {
        _targetWall = wall;
        findWallGround();
        _multipleMode = true;
    }

    public override void start()
    {
        if (!isNextToWall())
            unit.changeState(new Walking(unit.transform.position, _groundNextToWall.transform.position, unit));
        else
            unit.changeState(new Drilling(_targetWall, 1, unit));
    }

    public override void execute()
    {
        if (isNextToWall() && unit.getState().GetType() != typeof(Drilling))
            unit.changeState(new Drilling(_targetWall, 1, unit));
    }

    public override bool isFinished()
    {
        return (_targetWall == null);
    }

    private void findWallGround()
    {
        foreach (GameObject go in _targetWall.neighbours.Values)
        {
            if (go == null) continue;

            if (go.CompareTag("Ground"))
            {
                _groundNextToWall = go.GetComponent<Ground>();
                break;
            }
        }
        if (_groundNextToWall == null && unit != null)
            end();
        return;
    }

    private bool isNextToWall()
    {
        if (_groundNextToWall == null)
            findWallGround();

        Debug.Log("drilltask for " + unit);

        return unit.isAtPosition(_groundNextToWall.transform.position);
    }
}


