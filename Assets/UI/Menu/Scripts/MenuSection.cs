using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class MenuSection : MonoBehaviour
{
    [SerializeField] private List<MenuItem> menuItems = new List<MenuItem>();
    [Tooltip("Time in seconds it takes each menu item in order of the list above to start to disapear after the previous")]
    [SerializeField] private float ItemDisappearSpacing = 0.25f;

    [Tooltip("Time in seconds it takes each menu item in order of the list above to start to reappear after the previous")]
    [SerializeField] private float ItemReappearSpacing = 0.25f;

    [SerializeField] private UnityEvent OnOpen;
    [SerializeField] private UnityEvent OnClose;

    private bool m_IsOpen;

    public bool IsOpen
    {
        get { return m_IsOpen; }
        set
        {
            if (m_IsOpen == value)
                return;

            if (m_IsOpen)
                OnOpen.Invoke();
            else
                OnClose.Invoke();

            m_IsOpen = value;
        }
    }


    public void Open(bool instantOpen = false)
    {
        StartCoroutine(OpenCouroutine(instantOpen));
    }
    private IEnumerator OpenCouroutine(bool instantOpen)
    {
        IsOpen = true;

        foreach(MenuItem menuItem in menuItems)
        {
            menuItem.IsVisible = true;
            if(!instantOpen)
                yield return new WaitForSeconds(ItemReappearSpacing);
        }
    }

    public void Close(bool instantClose = false)
    {
        StartCoroutine(CloseCouroutine(instantClose));
    }
    private IEnumerator CloseCouroutine(bool instantClose)
    {
        foreach (MenuItem menuItem in menuItems)
        {
            menuItem.IsVisible = false;
            if (!instantClose)
                yield return new WaitForSeconds(ItemDisappearSpacing);
        }

        IsOpen = false;

    }



}



