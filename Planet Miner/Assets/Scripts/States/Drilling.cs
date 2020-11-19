using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drilling : State
{
    private Wall _targetWall;
    private float _drillDamage;


    public Drilling(Wall targetWall, float drillDamage)
    {
        _targetWall = targetWall;
        _drillDamage = drillDamage;
    }

    public override void run()
    {
        _targetWall.doDamage(_drillDamage);
        if (_targetWall.health < 0)
        {
            GameObject.FindObjectOfType<TerrainControl>().replaceTerrainObject(_targetWall.gameObject, "Ground");
        }
    }
}
