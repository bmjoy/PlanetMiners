using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    public static GameObject cursorObject;

    private void Awake()
    {
        cursorObject = this.gameObject;
    }
}
