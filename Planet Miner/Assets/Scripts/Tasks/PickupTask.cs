using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTask : Task
{

    Resource _resource;

    public PickupTask(Resource resource)
    {
        _resource = resource;
    }

    public override void execute()
    {
        if (isAtResource() && unit.getState().GetType() != typeof(Pickup))
            unit.changeState(new Pickup(_resource, unit));
    }

    public override bool isFinished()
    {
        return unit.hasItem(_resource);
    }

    public override void start()
    {
        if (!isAtResource())
            unit.changeState(new Walking(unit.transform.position, _resource.transform.position, this.unit));
        else
            unit.changeState(new Pickup(_resource, unit));
    }

    private bool isAtResource()
    {
        if (_resource == null)
            return false;

        return (unit.isAtPosition(_resource.transform.position));
    }
}
