using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public Camera worldCamera;
    public UnitControl unitControl;

    private bool mouseIsPressed = false;
    private float timeMouseHeld;
    private float timeToHoldMouse = 3f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (isMouseHeld())
                updateSelectField();
            else
                singlePress();

        if (Input.GetMouseButtonDown(1))
            righClick();

        if (Input.GetMouseButtonUp(0))
            mouseUp();
    }

    private void singlePress()
    {
        GameObject hit = getRayTarget(Input.mousePosition);
        if (hit.CompareTag("Unit"))
            unitControl.selectSingleUnit(hit.transform.GetComponent<Unit>());
        else
        if (hit.CompareTag("Ground"))
            unitControl.deselectUnits();
    }

    private void mouseHold()
    {

    }

    private void righClick()
    {
        GameObject hit = getRayTarget(Input.mousePosition);

        if (unitControl.hasUnitsSelected())
            switch (hit.tag)
            {
                case "Ground":
                    unitControl.assignTaskToSelected("WalkTask",hit);
                    break;
                case "Wall":
                    unitControl.assignTaskToSelected("DrillTask", hit);
                    break;
            }

    }

    private void updateSelectField()
    {

    }

    private void mouseUp()
    {
        if (isMouseHeld())
        {
            mouseIsPressed = false;

        }

    }

    private bool isMouseHeld()
    {
        if (timeMouseHeld >= timeToHoldMouse)
            return true;
        else
            timeMouseHeld += Time.deltaTime;

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
        Physics.Raycast(ScreenToRay(mousePosition), out RaycastHit hit);
        return hit.transform.gameObject;
    }
}
