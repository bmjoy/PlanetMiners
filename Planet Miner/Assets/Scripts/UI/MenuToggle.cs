using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject[] menus;
    private GameObject currentMenu;

    private void Start()
    {
        toggleMenu(menus[0]);
    }

    public void toggleMenu(GameObject newMenu)
    {
        if(currentMenu != null)
            currentMenu.SetActive(false);
        
        foreach(GameObject menu in menus)
        {
            if (menu == newMenu)
            {
                newMenu.SetActive(true);
                currentMenu = newMenu;
            }
        }
    }
}
