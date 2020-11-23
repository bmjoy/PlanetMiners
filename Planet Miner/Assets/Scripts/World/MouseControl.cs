using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseControl : MonoBehaviour
{
    public Camera worldCamera;
    public TerrainControl terrainControl;
    public UnitControl unitControl;
    public UIControl uiControl;

    private bool _mouseIsPressed = false;
    [SerializeField]
    private float _timeMouseHeld;
    [SerializeField]
    private float _timeToHoldMouse = 1f;

    private MouseMode _mouseMode = MouseMode.none;

    public LayerMask targetLayer;

    private ActionButton lastButtonPressed = null;

    public Texture2D arrowCursor;

    [Header("Selection rect")]
    public RectTransform selectionRect;
    [Header("Selection Positions")]
    [SerializeField]
    private Vector2 _selectionStartPosition;
    [SerializeField]
    private Vector2 _selectionEndPosition;



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
            if (!isMouseHeld())
                leftClick();

        if (Input.GetMouseButton(0))
            if (isMouseHeld())
                updateSelectField(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
            righClick();

        if (Input.GetMouseButtonUp(0))
            mouseUp();
    }

    public void leftClick()
    {
        GameObject hit = getRayTarget(Input.mousePosition);

        _selectionStartPosition = Input.mousePosition;

        if (hit == null)
            return;

        switch (_mouseMode)
        {
            case MouseMode.none:
                if (hit.CompareTag("Unit"))
                {
                    unitControl.selectSingleUnit(hit.transform.GetComponent<Unit>());
                    _mouseMode = MouseMode.unitSelected;
                    uiControl.changeSideMenu("UnitMenu");
                }
                break;

            case MouseMode.unitSelected:
                if (isMouseOnUI())
                    return;

                switch (hit.tag)
                {
                    case "Unit":
                        unitControl.deselectUnits();
                        unitControl.selectSingleUnit(hit.transform.GetComponent<Unit>());
                        _mouseMode = MouseMode.unitSelected;
                        break;
                    case "Ground":
                        unitControl.assignTaskToSelected("WalkTask", hit);
                        break;

                    case "Wall":
                        unitControl.assignTaskToSelected("DrillTask", hit);
                        break;

                    case "Resource":
                        unitControl.assignTaskToSelected("PickupTask", hit);
                        break;
                    case "Rubble":
                        unitControl.assignTaskToSelected("DigTask", hit);
                        break;
                }

                break;
            case MouseMode.waitingForTarget:
                unitControl.assignTaskToSelected(lastButtonPressed.actionName, hit);
                _mouseMode = MouseMode.unitSelected;
                break;
        }


    }

    public void clickedActionButton(ActionButton actionButton)
    {
        lastButtonPressed = actionButton;



        switch (actionButton.targetSystem)
        {
            case ActionButton.TargetSystem.UnitControl:
                if (actionButton.actionType.Equals(ActionButton.ActionType.needTarget))
                {
                    _mouseMode = MouseMode.waitingForTarget;

                    targetLayer = actionButton.targetLayer;
                }
                else if (actionButton.actionType.Equals(ActionButton.ActionType.instant))
                {
                    if (actionButton.actionName == "SpawnUnit")
                    {
                        EventManager.current.spawnUnit();
                    }else if(actionButton.actionName == "DespawnUnit")
                    {
                        EventManager.current.deSpawnUnit();
                    }

                    unitControl.assignTaskToSelected(lastButtonPressed.actionName, null);
                }
                break;

            case ActionButton.TargetSystem.UIControl:
                if (actionButton.actionName == "ToBuildMenu")
                    uiControl.changeSideMenu("BuildMenu");
                break;

            case ActionButton.TargetSystem.ConstructionControl:

                break;

            case ActionButton.TargetSystem.BuildingControl:

                break;
        }
    }

    private bool isMouseOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void mouseHold()
    {

    }

    private void righClick()
    {
        unitControl.deselectUnits();
        uiControl.changeSideMenu("ControlMenu");
        _mouseMode = MouseMode.none;
        targetLayer = 1 << 16;

    }

    private void updateSelectField(Vector3 mousePosition)
    {
        if (!selectionRect.gameObject.activeInHierarchy)
            selectionRect.gameObject.SetActive(true);

        float width = mousePosition.x - _selectionStartPosition.x;
        float height = mousePosition.y - _selectionStartPosition.y;

        selectionRect.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionRect.anchoredPosition = _selectionStartPosition + new Vector2(width / 2, height / 2);
    }

    private void mouseUp()
    {
        if (isMouseHeld())
        {
            unitControl.deselectUnits();

            selectionRect.gameObject.SetActive(false);

            _mouseIsPressed = false;

            Vector2 minPos = selectionRect.anchoredPosition - (selectionRect.sizeDelta / 2);
            Vector2 maxPos = selectionRect.anchoredPosition + (selectionRect.sizeDelta / 2);

            List<Unit> selected = new List<Unit>();

            foreach (Unit unit in unitControl.units())
            {
                Vector3 screenpos = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (screenpos.x > minPos.x && screenpos.x < maxPos.x
                    && screenpos.y > minPos.y && screenpos.y < maxPos.y)
                {
                    selected.Add(unit);
                }
            }

            unitControl.selectMultipleUnits(selected);
            if (unitControl.hasUnitsSelected())
                _mouseMode = MouseMode.unitSelected;
            else
                _mouseMode = MouseMode.none;

            _selectionEndPosition = new Vector3(0, 0, 0);
        }
        _timeMouseHeld = 0;
    }


    private bool isMouseHeld()
    {
        if (_timeMouseHeld >= _timeToHoldMouse)
            return true;
        else
            _timeMouseHeld += Time.deltaTime;

        return false;
    }

    private Ray screenToRay(Vector3 mousePosition)
    {
        return worldCamera.ScreenPointToRay(mousePosition);
    }

    private Vector3 screenToWorld(Vector3 mousePosition)
    {
        return worldCamera.ScreenToWorldPoint(mousePosition);
    }

    private GameObject getRayTarget(Vector3 mousePosition)
    {
        if (Physics.Raycast(screenToRay(mousePosition), out RaycastHit hit))
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


    private void onUnitSelected()
    {

    } 

}
