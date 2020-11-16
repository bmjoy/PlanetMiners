using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digging : State
{
    Rubble _rubble;
    float _digSpeed;
    public Digging(Rubble rubble,float digSpeed)
    {
        _rubble = rubble;
        _digSpeed = digSpeed;
    }

    public override void run()
    {
        _rubble.doDamage(_digSpeed);

        if (_rubble.health <= 0)
        {
            GameObject.FindObjectOfType<TerrainControl>().removeRubble(_rubble);

        }
    }
}
