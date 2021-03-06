using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public static MenuManager InstanceMM;

    [SerializeField] Menu[] menus;

    [SerializeField] string BackToTitleName;

    private void Awake()
    {
        InstanceMM = this;
        for (int i = 0; i < menus.Length; i++)
            CloseMenu(menus[i]);
    }

    private void Start()
    {
        OpenMenu(BackToTitleName);
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].Name == menuName)
            {
                menus[i].Open();
            }
            else if(menus[i].Openned)
            {
                CloseMenu(menus[i]);
            }
        }

        //SM.GetASound("ButtonUIClick", transform, true);
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].Openned)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
        //SM.GetASound("ButtonUIClick", transform, true);
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void Quit()
    {
        //SM.GetASound("ButtonUIClick", transform, true);
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu(BackToTitleName);
        }
    }
}
