using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble : WorldBlock
{

    public GameObject dropResource;

    [Range(0, 1)]
    public float resourceDropChance = 0;

    private void Start()
    {
        health = 5f;
    }
}
