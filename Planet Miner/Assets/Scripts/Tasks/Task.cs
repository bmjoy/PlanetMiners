using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Task
{
    private Unit _unit;
    public Unit unit
    {
        get => _unit;
        set => _unit = value;
    }

    public abstract void start();

    public abstract void execute();

    public virtual void end()
    {
        _unit.changeState(new Idle());
    }

    public abstract bool isFinished();

}
