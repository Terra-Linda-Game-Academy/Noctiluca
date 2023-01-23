using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<MenuSection> menuSections = new List<MenuSection>();

    private MenuSection currentSection;

    [SerializeField] private MenuSection startingSection;

    [SerializeField] private RectTransform sectionTitleBackground;
    [SerializeField] private TextMeshProUGUI sectionTitle;
    [SerializeField] private TextMeshProUGUI sectionTitleShadow;

    public void ChangeSection(MenuSection targetSection)
    {
        currentSection.Close();
        currentSection = targetSection;
        currentSection.Open();

        sectionTitle.text = currentSection.sectionName;
        sectionTitleShadow.text = currentSection.sectionName;
        
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

        currentSection.gameObject.SetActive(true);
        currentSection.Open(true);
    }
    public void Update() {
        //lerp the section title background x size to the sectionTitle's x size + 25
        sectionTitleBackground.sizeDelta = new Vector2(Mathf.Lerp(sectionTitleBackground.sizeDelta.x, sectionTitle.renderedWidth + 25, Time.deltaTime * 5), sectionTitleBackground.sizeDelta.y);
        
    }

    public void LoadSections() {
        menuSections.Clear();

        MenuSection[] sections = Resources.FindObjectsOfTypeAll<MenuSection>();
        menuSections.AddRange(sections);
    }
}





/*
[Serializable]
public class MenuSection
{
    public MenuPanel menuPanel;
}
*/
