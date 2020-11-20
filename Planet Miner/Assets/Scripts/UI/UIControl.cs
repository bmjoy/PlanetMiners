using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject sideMenus;
    private GameObject currentOpenMenu = null;

    private void Start()
    {
        changeSideMenu("ControlMenu");
    }

    public void changeSideMenu(string MenuName)
    {
        foreach (Transform menu in sideMenus.GetComponentsInChildren<Transform>(true))
        {
            if (menu.name == MenuName)
            {
                if (currentOpenMenu != null)
                    currentOpenMenu.SetActive(false);

                currentOpenMenu = menu.gameObject;
                currentOpenMenu.SetActive(true);
                return;
            }
        }
    }

}
