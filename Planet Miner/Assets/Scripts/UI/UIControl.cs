using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl uIControl;
    public GameObject sideMenus;
    private GameObject currentOpenMenu = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI[] resourcesTexts;

    public Text powerGridText;

    private void Awake()
    {
        uIControl = this;
    }

    private void Start()
    {
        changeSideMenu("ControlMenu");
        EventManager.current.onResourceChanged += UpdateResourceBar;
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

    public void updatePowerGridText(string text) => powerGridText.text = text;

    public void UpdateResourceBar(int index, int value)
    {
        resourcesTexts[index].text = value.ToString();
    }

}
