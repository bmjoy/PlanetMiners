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

        if (Pathfinding.checkForPath(unit.transform.position, _rubble.transform.position))
            unit.changeState(new Walking(unit.transform.position, _rubble.transform.position, unit));
        else
            _taskEnded = true;
    }

    public override void execute()
    {
        if (unit.isAtPosition(_rubble.transform.position) && unit.getState().GetType() != typeof(Digging))
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


}
