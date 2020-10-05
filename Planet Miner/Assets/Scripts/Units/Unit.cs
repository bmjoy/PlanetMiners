using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    private State _state = new Idle();

    private Task _task = null;

    public float moveSpeed
    {
        get { return _moveSpeed * Time.deltaTime; }
    }




    private void Update()
    {
        if (_task == null)
            return;

        _task.execute();

        checkTaskFinished();

        if (_state != null)
            _state.run();
    }

    private bool checkTaskFinished()
    {
        if (_task.isFinished())
        {
            _task.end();
            _task = null;
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
            _task.end();

        _task = task;
        _task.unit = this;

        _task.start();
    }

    public bool hasTask
    {
        get => (_task != null);
    }

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
