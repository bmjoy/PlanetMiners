using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnUnit : MonoBehaviour
{
    Unit unit;
    private void Update()
    {
        if (unit == null)
            unit = FindObjectOfType<Unit>();

        if (unit != null)
        {
            Vector3 position = unit.transform.position;

            position.y = 10;

            transform.position = position;
        }
    }
}
