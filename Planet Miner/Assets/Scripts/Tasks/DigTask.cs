using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTask : Task
{

    private Rubble _rubble;
    private float _digDamage;
    private bool rubbleCleared = false;


    public DigTask(Rubble rubble, float digDamage)
    {
        _rubble = rubble;
        _digDamage = digDamage;
        _multipleMode = true;
    }
    public override void execute()
    {
        if (_rubble.health <= 0 && !rubbleCleared)
        {
            GameObject.FindObjectOfType<TerrainControl>().removeRubble(_rubble);
            rubbleCleared = true;
        }
        else
        if (_rubble == null)
            end();

        if (unit.isAtPosition(_rubble.transform.position))
        {
            _rubble.doDamage(_digDamage);
        }
    }

    public override bool isFinished()
    {
        return rubbleCleared;
    }

    public override void start()
    {
        unit.changeState(new Walking(unit.transform.position, _rubble.transform.position, unit));
    }
}
