using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Task
{
    private Unit _unit;
    protected bool _taskEnded = false;

    protected bool _multipleMode = false;
    public Unit unit
    {
        get => _unit;
        set => _unit = value;
    }

    public bool multipleMode {get=> _multipleMode;}
    public abstract void start();

    public abstract void execute();

    public virtual void end()
    {
        
    }

    public abstract bool isFinished();

    internal Task clone()
    {
        return (Task) this.MemberwiseClone();
        
    }
}
