using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    private State _state;

    private Task _task = null;

    public Inventory _inventory;

    public string taskname;
    public string statename;

    private void Start()
    {
        _state = new Idle();
    }
    public float moveSpeed
    {
        get { return _moveSpeed * Time.deltaTime; }
    }

    public bool hasItem(Equipable item)
    {
        return (item.gameObject == _inventory.itemInHand());
    }

    public Inventory inventory
    {
        get => _inventory;
    }

    private void Update()
    {
        runTask();

        runState();

        if (_task != null)
            taskname = _task.ToString();
        if (_state != null)
            statename = _state.ToString();
    }

    private void runTask()
    {
        if (_task == null)
            return;

        if (checkTaskFinished())
            return;

        _task.execute();

    }

    private void runState()
    {
        if (_state != null)
            _state.run();
    }

    private bool checkTaskFinished()
    {
        if (_task.isFinished())
        {
            endTask();
            return true;
        }
        return false;
    }

    public State getState()
    {
        return _state;
    }

    public void changeTask(Task task)
    {
        if (task == null)
            return;

        if (_task != null)
            endTask();

        _task = task;
        _task.unit = this;

        checkTaskFinished();



        _task.start();
    }

    public void endTask()
    {
        _task.end();
        _task = null;
        changeState(new Idle());
        taskname = "";
    }

    public bool hasTask
    {
        get => (_task != null);
    }
    public Task task { get => _task; }

    public void changeState(State state)
    {
        _state = state;

    }

    public bool isAtPosition(Vector3 pos)
    {
        return (transform.position.x == pos.x && transform.position.z == pos.z);
    }

    public void setSelected(bool v)
    {
        if (v)
            selectUnit();
        else
            deselectUnit();
    }
    private void selectUnit()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void deselectUnit()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }


}
