using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : WorldBlock
{
    public GameObject dropObject;

    public override void destroyed()
    {
        if (dropObject != null)
            Instantiate(dropObject, new Vector3(transform.position.x, .5f, transform.position.z), Quaternion.identity, transform.parent);
    }

    private void Start()
    {
        health = 5;
    }
    
    
}
