using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulTask : Task
{
    private GameObject _objectToHaul;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private bool isAtStartPosition = false;
    private bool hasItem = false;
    private bool isHauling = false;


    public HaulTask(GameObject objectToHaul, Vector3 endPosition )
    {
        _objectToHaul = objectToHaul;
        _startPosition = _objectToHaul.transform.position;
        _endPosition = endPosition;
    }

    public override void execute()
    {
        if (unit.isAtPosition(_startPosition) && !isAtStartPosition)
        {
            unit.changeState(new Pickup(_objectToHaul.GetComponent<Resource>(), unit));
            isAtStartPosition = true;
        }

        if (unit.inventory.itemInHand() == _objectToHaul && isAtStartPosition && !hasItem)
        {
            unit.changeState(new Walking(_startPosition, _endPosition, unit));
            hasItem = true;
            isHauling = true;
        }

        if(unit.isAtPosition(_endPosition) && hasItem)
        {
            Debug.Log($"Dropping {_objectToHaul.name} on ground");
            unit.changeState(new Drop(unit));
            hasItem = false;
            Debug.Log($"Dropped {_objectToHaul.name} on ground");
        }
    }

    public override bool isFinished()
    {
        return (unit.inventory.itemInHand() != _objectToHaul && isHauling);
    }

    public override void start()
    {
        unit.changeState(new Walking(unit.transform.position, _startPosition, unit));
    }
}
