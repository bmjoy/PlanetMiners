using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystem
{
    static TaskSystem _instance;

    private static WorldGen worldGen;

    private static Queue<Task> globalQueue = new Queue<Task>();
    private TaskSystem()
    {

    }

    public static TaskSystem instance
    {
        get
        {
            if (_instance == null)
                _instance = new TaskSystem();

            return _instance;
        }
    }

    public static void enqueTask(Task task)
    {
        if (task == null)
            return;
        globalQueue.Enqueue(task);
    }

    public static void asignTasks()
    {
        foreach (Unit unit in worldGen.getUnits())
        {
            if (globalQueue.Count > 0 && !unit.hasTask)
                unit.changeTask(globalQueue.Dequeue());
            else
                return;
        }
    }
}
