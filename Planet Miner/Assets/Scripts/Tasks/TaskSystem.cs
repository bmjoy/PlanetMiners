using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskSystem
{
    private Queue<Task> globalQueue = new Queue<Task>();
    public TaskSystem()
    {

    }

    public void enqueTask(Task task)
    {
        if (task == null)
            return;
        globalQueue.Enqueue(task);
    }

    public void asignTasks(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            if (globalQueue.Count > 0 && !unit.hasTask)
                unit.changeTask(globalQueue.Dequeue());
            else
                return;
        }
    }

    public void createHaulTask(List<Resource> list)
    {
        foreach(Resource r in list)
        {
            enqueTask(new HaulTask(r.gameObject, r.transform.position - Vector3.back));
        }
    }
}
