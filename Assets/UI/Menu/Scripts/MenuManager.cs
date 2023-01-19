using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<MenuSection> menuSections = new List<MenuSection>();

    private MenuSection currentSection;

    [SerializeField] private MenuSection startingSection;

    public void ChangeSection(MenuSection targetSection)
    {
        currentSection.Close();
        currentSection = targetSection;
        currentSection.Open();
    }

    public void Start()
    {
        currentSection = startingSection;
            

        if (startingSection == null)
            startingSection = menuSections[0];


        foreach(MenuSection menuSection in menuSections)
        {
            menuSection.gameObject.SetActive(true);
            if(menuSection != currentSection)
                menuSection.Close(true);
        }

        currentSection.Open(true);
    }
}





/*
[Serializable]
public class MenuSection
{
    public MenuPanel menuPanel;
}
*/
