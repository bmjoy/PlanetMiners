using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControl : MonoBehaviour
{
    [Header("World Objects")]
    public TerrainControl terrainControl;
    [SerializeField]
    private TaskSystem taskSystem;
    [SerializeField]
    private Pathfinding pathfinding;
    public UnitControl unitControl;
    public MouseControl mouseControl;
    //public SoundControl soundControl;

    [Header("Loop timer values")]
    public float taskSystemLoopTime = 0;

    private void Start()
    {
        pathfinding = new Pathfinding();
        taskSystem = new TaskSystem();

        terrainControl.generateWorld();

        foreach (Node n in terrainControl.getAllNodeObjects())
            Pathfinding.addNode(n);
    }

    IEnumerator taskSystemLoop()
    {
        yield return new WaitForSeconds(taskSystemLoopTime);

        taskSystem.asignTasks(unitControl.units());
        StartCoroutine(taskSystemLoop());
    }
}
