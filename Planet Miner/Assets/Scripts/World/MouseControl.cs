using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseControl : MonoBehaviour
{
    public Camera worldCamera;
    public UnitControl unitControl;

    private bool _mouseIsPressed = false;
    private float _timeMouseHeld;
    private float _timeToHoldMouse = 3f;

    private MouseMode _mouseMode = MouseMode.none;

    public LayerMask targetLayer;


    private ActionButton lastButtonPressed = null;

    public Texture2D arrowCursor;

    private void Start()
    {
        targetLayer = 1 << 16;
    }
    public enum MouseMode
    {
        none,
        unitSelected,
        waitingForTarget
    }

    private string _targetTag;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (isMouseHeld())
                updateSelectField();
            else
                leftClick(getRayTarget(Input.mousePosition));

        if (Input.GetMouseButtonDown(1))
            righClick();

        if (Input.GetMouseButtonUp(0))
            mouseUp();
    }

    public void leftClick(GameObject hit)
    {
        if (hit == null)
            return;

        switch (_mouseMode)
        {
            case MouseMode.none:
                if (hit.CompareTag("Unit"))
                {
                    unitControl.selectSingleUnit(hit.transform.GetComponent<Unit>());
                    _mouseMode = MouseMode.unitSelected;
                }
                break;

            case MouseMode.unitSelected:
                switch (hit.tag)
                {
                    case "Ground":
                        unitControl.assignTaskToSelected("WalkTask", hit);
                        break;

                    case "Wall":
                        unitControl.assignTaskToSelected("DrillTask", hit);
                        break;

                    case "Resource":
                        unitControl.assignTaskToSelected("PickupTask", hit);
                        break;

                    case "ActionButton":
                        ActionButton actionButton = hit.GetComponent<ActionButton>();
                        lastButtonPressed = actionButton;

                        if (actionButton.actionType.Equals(ActionButton.ActionType.needTarget))
                        {
                            _mouseMode = MouseMode.waitingForTarget;

                            targetLayer = actionButton.targetLayer;

                            
                        }
                        else if (actionButton.actionType.Equals(ActionButton.ActionType.instant))
                        {
                            unitControl.assignTaskToSelected(lastButtonPressed.taskName, null);
                        }
                        break;
                }

                break;
            case MouseMode.waitingForTarget:
                unitControl.assignTaskToSelected(lastButtonPressed.taskName, hit);
                _mouseMode = MouseMode.unitSelected;
                changePointer(arrowCursor);
                break;
        }


    }

    private void mouseHold()
    {

    }

    private void righClick()
    {
        unitControl.deselectUnits();
        _mouseMode = MouseMode.none;
        targetLayer = 1 << 16;
        changePointer(arrowCursor);

    }

    private void updateSelectField()
    {

    }

    private void mouseUp()
    {
        if (isMouseHeld())
        {
            _mouseIsPressed = false;

        }

    }

    private bool isMouseHeld()
    {
        if (_timeMouseHeld >= _timeToHoldMouse)
            return true;
        else
            _timeMouseHeld += Time.deltaTime;

        return false;
    }

    private Ray ScreenToRay(Vector3 mousePosition)
    {
        return worldCamera.ScreenPointToRay(mousePosition);
    }

    private Vector3 ScreenToWorld(Vector3 mousePosition)
    {
        return worldCamera.ScreenToWorldPoint(mousePosition);
    }

    private GameObject getRayTarget(Vector3 mousePosition)
    {
        if (Physics.Raycast(ScreenToRay(mousePosition), out RaycastHit hit))
        {
            return hit.transform.gameObject;
        }

        return null;
    }

    public void changePointer(Texture2D cursorTexture)
    {
        if (cursorTexture == null) return;
        Cursor.SetCursor(cursorTexture, new Vector2(0, 0), CursorMode.Auto);
    }

    
}
