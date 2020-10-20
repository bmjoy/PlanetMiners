using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : WorldBlock
{
    public GameObject dropObject;

    public override void destroyed()
    {
        
    }

    private void Start()
    {
        health = 5;
    }
    
    
}
