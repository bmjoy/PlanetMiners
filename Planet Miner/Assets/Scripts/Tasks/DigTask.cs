using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTask : Task
{

    private Rubble _rubble;
    private float _digDamage;


    public DigTask(Rubble rubble)
    {
        _rubble = rubble;
        _multipleMode = true;
    }

    public override void start()
    {
        _digDamage = 1f;
        if (!isAtRubble())
            if (Pathfinding.checkForPath(unit.transform.position, _rubble.transform.position))
                unit.insertTask(TaskSystem.createWalkTask(_rubble.transform.position));
            else
                _taskEnded = true;
        else
            unit.changeState(new Digging(_rubble, _digDamage));

    }

    public override void execute()
    {
        if (isAtRubble() && unit.getState().GetType() != typeof(Digging))
        {
            unit.changeState(new Digging(_rubble, _digDamage));
        }
    }

    public override bool isFinished()
    {
        if (_rubble == null || _taskEnded)
            return true;

        return false;
    }

    private bool isAtRubble()
    {
        if(_rubble == null)
        {
            _taskEnded = true;
            return true;
        }

        return unit.isAtPosition(_rubble.transform.position);
    }




}
