using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    private State _state;

    private Task _currentTask = null;
    private Queue<Task> _taskQueue = new Queue<Task>();
    private bool doingTask = false;

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
        taskLoop();

        runState();

        if (_currentTask != null)
            taskname = _currentTask.ToString();
        if (_state != null)
            statename = _state.ToString();
    }



    #region state

    private void runState()
    {
        if (_state != null)
            _state.run();
    }

    public State getState()
    {
        return _state;
    }

    public void changeState(State state)
    {
        _state = state;

    }

    #endregion

    #region tasks
    public void enqueueTask(Task task)
    {
        task.unit = this;
        _taskQueue.Enqueue(task);
    }

    public Task deQueueTask()
    {
        if (_taskQueue.Count == 0)
            return null;

        return _taskQueue.Dequeue();
    }

    public void insertTask(Task task)
    {
        enqueueTask(task);
        enqueueTask(currentTask);
        doingTask = false;
        nextTask();
        startTask();
    }
    private void startTask()
    {
        _currentTask.start();
        doingTask = true;
    }

    private void executeTask()
    {
        _currentTask.execute();
    }

    private bool checkTaskFinished()
    {
        return _currentTask.isFinished();
    }

    private void endTask()
    {
        doingTask = false;
        _currentTask.end();
        _currentTask = null;
        changeState(new Idle());
        taskname = "";

    }

    private void nextTask()
    {
        _currentTask = deQueueTask();
    }

    public Task currentTask
    {
        get => _currentTask;
    }

    public bool hasTask
    {
        get => (_currentTask != null);
    }

    private void taskLoop()
    {
        if (!hasTask)
            nextTask();

        if (hasTask && !doingTask)
            startTask();

        if (hasTask && doingTask)
        {
            if (checkTaskFinished())
                endTask();
            else
                executeTask();
        }
    }


    #endregion

    #region position

    public Vector3 position
    {
        get => transform.position;
    }

    public bool isAtPosition(Vector3 pos)
    {
        return (transform.position.x == pos.x && transform.position.z == pos.z);
    }

    #endregion


    #region selection
    public void toggleSelected(bool v)
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
    #endregion
}
