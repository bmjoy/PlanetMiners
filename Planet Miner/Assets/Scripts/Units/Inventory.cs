using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform _handSlotPosition;
    private GameObject _handSlot;
    private List<GameObject> _tools = new List<GameObject>();
    private GameObject _weapon;


    private int _maxTools = 1;

    public GameObject itemInHand()
    {
        return _handSlot;
    }

    public void pickUpItem(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        if (_handSlot != null)
            dropItem();

        _handSlot = gameObject;
        _handSlot.transform.SetParent(_handSlotPosition);
        _handSlot.transform.position = _handSlotPosition.position;
        
    }

    public void dropItem()
    {
        if (_handSlot == null)
            return;

        Vector3 dropPosition = transform.position;
        dropPosition.y = 0f;

        _handSlot.transform.SetParent(transform.parent);
        _handSlot.transform.position = dropPosition;
    }

    public void addTool(GameObject toolAdded)
    {
        if (_tools.Count < _maxTools)
        {
            foreach (GameObject tool in _tools)
            {
                if (tool == null || tool.name != toolAdded.name)
                    _tools.Add(toolAdded);
               
            }
        }
    }

    public void removeTool(string toolname)
    {
        foreach (GameObject tool in _tools)
        {
            if (tool.name == toolname)
            {
                Vector3 dropPosition = transform.position;
                dropPosition += transform.forward;

                Instantiate(tool, dropPosition, Quaternion.identity);
            }
        }
    }

    public void equipTool(string tool)
    {
        if (_handSlot == null)
            foreach (GameObject t in _tools)
            {
                if (t.name == tool)
                    _handSlot = t;
            }

        else if (_handSlot.CompareTag("Resource"))
            dropItem();
        else if (_handSlot.CompareTag("Weapon"))
            unequipWeapon(_handSlot.name);
    }

    public void unequipTool(string tool)
    {
        if (_handSlot != null && _handSlot.name == tool)
            _handSlot = null;
    }


    public void addWeapon(GameObject weapon)
    {
            _weapon = weapon;
    }

    public void removeWeapon(string weapon)
    {
        if (_weapon.name == weapon)
            _weapon = null;
    }

    public void equipWeapon(string weapon)
    {
        if (_handSlot == null)
        {
            if (_weapon.name == weapon)
                _handSlot = _weapon;
        }

        else if (_handSlot.CompareTag("Resource"))
            dropItem();
        else if (_handSlot.CompareTag("Tool"))
            unequipTool(_handSlot.name);
    }

    public void unequipWeapon(string weapon)
    {
        if(_handSlot != null && _handSlot.name == weapon)
            _handSlot = null;
    }
}
