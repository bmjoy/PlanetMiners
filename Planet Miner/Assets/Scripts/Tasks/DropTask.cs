using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTask : Task
{
    public override void execute()
    {
       
    }

    public override bool isFinished()
    {
        return !unit.hasItem(unit.inventory.itemInHand().GetComponent<Equipable>());
    }

    public override void start()
    {
        unit.changeState(new Drop(unit));

    }
}
