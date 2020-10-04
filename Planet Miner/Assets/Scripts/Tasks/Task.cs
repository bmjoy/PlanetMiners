using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    private Unit _unit;

    public Task(Unit unit)
    {
        _unit = unit;
    }
    public Unit unit
    {
        get => _unit;
    }

    public abstract void start();

    public abstract void execute();

    public virtual void end()
    {
        _unit.changeState(new Idle());
    }

    public abstract bool isFinished();

}
