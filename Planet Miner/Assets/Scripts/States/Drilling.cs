﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drilling : State
{
    private Wall _targetWall;
    private float _drillDamage;
    private Unit _unit;

    public Drilling(Wall targetWall, float drillDamage,Unit unit)
    {
        _unit = unit;
        _targetWall = targetWall;
        _drillDamage = drillDamage;

    }

    public override void execute()
    {
        _targetWall.doDamage(_drillDamage);
        if (_targetWall.health < 0)
        {
            GameObject.FindObjectOfType<WorldGen>().replaceWorldObject(_targetWall.gameObject, "Ground");
            _unit.changeState(new Idle());
        }
    }
}