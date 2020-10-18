using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : State
{
    private Resource _resource;
    private Unit _unit;

    public Pickup(Resource resource,Unit unit)
    {
        _resource = resource;
        _unit = unit;
    }
    public override void run()
    {
        _unit.inventory.pickUpItem(_resource.gameObject);
    }
}
