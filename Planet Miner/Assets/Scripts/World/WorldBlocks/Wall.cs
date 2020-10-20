using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : WorldBlock
{
    public GameObject rubbleObject;

    private void Start()
    {
        health = 5;
    }


}
