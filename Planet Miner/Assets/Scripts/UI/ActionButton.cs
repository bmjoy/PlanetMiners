using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionButton : MonoBehaviour
{
    public string actionName = "";

    public LayerMask targetLayer;

    public enum ActionType
    {
        instant,
        needTarget,
    }

    public enum TargetSystem
    {
        UnitControl,
        UIControl,
        ConstructionControl,
        BuildingControl
    }

    public ActionType actionType = ActionType.instant;

    public TargetSystem targetSystem = TargetSystem.UnitControl;
}
