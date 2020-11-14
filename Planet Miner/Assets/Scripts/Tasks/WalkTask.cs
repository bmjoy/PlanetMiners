using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTask : Task
{

    private Vector3 _targetPosition;

    public WalkTask(Vector3 targetposition)
    {
        _targetPosition = targetposition;
        _multipleMode = true;
    }
    public override void execute()
    {
        
    }

    public override bool isFinished()
    {
        return (unit.isAtPosition(_targetPosition));
    }

    public override void start()
    {
        if (!unit.isAtPosition(_targetPosition))
            unit.changeState(new Walking(unit.transform.position, _targetPosition, unit));
    }
}
