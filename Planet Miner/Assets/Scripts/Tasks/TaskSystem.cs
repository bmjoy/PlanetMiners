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
                unit.enqueueTask(globalQueue.Dequeue());
            else
                return;
        }
    }

    public void createMassHaulTask(List<Resource> list)
    {
        foreach(Resource r in list)
        {
            enqueTask(new HaulTask(r.gameObject, r.transform.position - Vector3.back));
        }
    }

    public static HaulTask createHaulTask(Resource resource, Vector3 position)
    {
        return null;
    }

    public static WalkTask createWalkTask(Vector3 targetPosition)
    {
        return new WalkTask(targetPosition);
    }

    public static DrillTask createDrillTask(Wall wall)
    {
        return new DrillTask(wall);
    }

    public static PickupTask createPickupTask(Resource resource)
    {
        return new PickupTask(resource);
    }

    public static DropTask createDropTask()
    {
        return new DropTask();
    }

    public static DigTask createDigTask(Rubble rubble )
    {
        return new DigTask(rubble);
    }
}
