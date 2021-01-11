using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnUnit : MonoBehaviour
{
    Unit unit;
    [SerializeField]
    private float camHeight;
    [SerializeField]
    private float camz;
    private void Update()
    {
        if (unit == null)
            unit = FindObjectOfType<Unit>();

        if (unit != null)
        {
            Vector3 position = unit.transform.position;

            position.y = camHeight;
            position.z -= camz;

            transform.position = position;
        }
    }
}
