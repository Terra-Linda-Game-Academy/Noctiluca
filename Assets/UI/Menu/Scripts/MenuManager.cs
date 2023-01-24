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


    [Header("Section Titles")]
    [SerializeField] private MenuItem titleItem;
    [SerializeField] private RectTransform sectionTitleBackground;
    [SerializeField] private TextMeshProUGUI sectionTitle;
    [SerializeField] private TextMeshProUGUI sectionTitleShadow;
    [SerializeField] private float TextBoxSizeLerpSpeed = 5f;

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

        sectionTitle.text = currentSection.sectionName;
        sectionTitleShadow.text = currentSection.sectionName;

        if(currentSection.sectionName.Length > 0)
            titleItem.IsVisible = true;
        else
            titleItem.IsVisible = false;
    }
    public void Update() {
        //lerp the section title background x size to the sectionTitle's x size + 25
        sectionTitleBackground.sizeDelta = new Vector2(Mathf.Lerp(sectionTitleBackground.sizeDelta.x, sectionTitle.text.Length>0 ? (sectionTitle.renderedWidth + 25) : 0, Time.deltaTime * TextBoxSizeLerpSpeed), sectionTitleBackground.sizeDelta.y);
        if(sectionTitle.text.Length > 0) {
            titleItem.IsVisible = true;
            if((sectionTitleBackground.sizeDelta.x < 1f)) {
                sectionTitleBackground.sizeDelta = new Vector2(1f, sectionTitleBackground.sizeDelta.y);
            }
        } else {
            titleItem.IsVisible = false;
        }
    }

    public void LoadSections() {
        menuSections.Clear();

        MenuSection[] sections = Resources.FindObjectsOfTypeAll<MenuSection>();
        menuSections.AddRange(sections);
    }

    public void Quit() {
        Application.Quit();
    }
}





/*
[Serializable]
public class MenuSection
{
    public MenuPanel menuPanel;
}
*/
