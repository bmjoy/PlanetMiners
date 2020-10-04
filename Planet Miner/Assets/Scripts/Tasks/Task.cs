using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    private State _state;
    private Unit _unit;

    public Task(Unit unit)
    {
        _unit = unit;
    }

    public State state
    {
        get => _state;
    }

    public virtual void start()
    {
        _unit.changeState(_state);
    }

    public abstract void execute();

    public virtual void end()
    {
        _state = new Idle();
    }

    public abstract bool isFinished();

}
