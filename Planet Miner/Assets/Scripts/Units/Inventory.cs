using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameObject _handSlot;
    private List<GameObject> _tools = new List<GameObject>();
    private GameObject _weapon;

    private int _maxTools = 1;

    public void pickUpItem(GameObject gameObject)
    {
        _handSlot = gameObject;
        Destroy(gameObject);
    }

    public void dropItem(string item)
    {
        Vector3 dropPosition = transform.position;
        dropPosition += transform.forward;
        Instantiate(_handSlot, dropPosition, Quaternion.identity);
    }

    public void addTool(GameObject toolAdded)
    {
        if (_tools.Count < _maxTools)
        {
            foreach (GameObject tool in _tools)
            {
                if (tool == null || tool.name != toolAdded.name)
                    _tools.Add(toolAdded);
                Destroy(toolAdded);
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
            dropItem(_handSlot.name);
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
            dropItem(_handSlot.name);
        else if (_handSlot.CompareTag("Tool"))
            unequipTool(_handSlot.name);
    }

    public void unequipWeapon(string weapon)
    {
        if(_handSlot != null && _handSlot.name == weapon)
            _handSlot = null;
    }
}
