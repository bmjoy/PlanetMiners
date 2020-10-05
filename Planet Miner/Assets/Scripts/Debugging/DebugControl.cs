using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControl : MonoBehaviour
{
    public bool debug;
    private void Update()
    {
        if (debug)
            PlayerPrefs.SetInt("Debugging", 1);
        else
            PlayerPrefs.SetInt("Debugging", 0);
    }
}
