using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : State
{

    private Unit _unit;

    public Drop(Unit unit)
    {
        _unit = unit;
    }
    public override void run()
    {
        GameObject.FindObjectOfType<TerrainControl>().addResourceToList(_unit.inventory.itemInHand().GetComponent<Resource>());
        _unit.inventory.dropItem();
    }
}
