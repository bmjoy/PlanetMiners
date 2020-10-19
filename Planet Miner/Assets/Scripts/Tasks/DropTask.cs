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
        return (unit.inventory.itemInHand() == null);
    }

    public override void start()
    {
        unit.changeState(new Drop(unit));

    }
}
