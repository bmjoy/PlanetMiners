using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabControl : MonoBehaviour
{
    public static PrefabControl current;

    private void Awake()
    {
        current = this;
    }

    [Header("Terrain")]
    public GameObject ground;
    public GameObject wall;
    public GameObject wallCrystal;
    public GameObject corner;
    public GameObject cornerInverse;

    [Header("Resources")]
    public GameObject ore;
    public GameObject crystal;

    [Header("Buildings")]
    public GameObject unitHub;
    public GameObject unitSpawn;
    public GameObject generator;

}
