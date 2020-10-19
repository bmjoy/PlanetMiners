using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public string taskName = "";

    public LayerMask targetLayer;

    public enum ActionType
    {
        instant,
        needTarget
    }

    public ActionType actionType = ActionType.instant;
}
