using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl uIControl;
    public GameObject sideMenus;
    private GameObject currentOpenMenu = null;

    public Text powerGridText;

    private void Start()
    {
        uIControl = this;
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
