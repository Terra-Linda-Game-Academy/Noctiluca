using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;


public class MenuItem : MonoBehaviour
{

    [HideInInspector] public Vector3 visiblePosition;
    private Canvas canvas;
    //private RectTransform rectTransform;

    [SerializeField] private float moveSpeed = 0.5f;

    //[SerializeField] private Transform hiddenPosition;
    [SerializeField] private MenuItemTransitionInfo transitionInfo;

    [SerializeField] private UnityEvent OnBecomeVisible;
    [SerializeField] private UnityEvent OnBecomeHidden;


    private bool m_IsVisible;

    public bool IsVisible
    {
        get { return m_IsVisible; }
        set {
            if (m_IsVisible == value)
                return;

            if (m_IsVisible)
                OnBecomeVisible.Invoke();
            else
                OnBecomeHidden.Invoke();

            m_IsVisible = value; 
        }
    }




    private void Start()
    {
        //rectTransform = GetComponent<RectTransform>();
        visiblePosition = transform.position; 
        canvas = transform.root.GetComponent<Canvas>();
        if(canvas == null)
        {
            throw new Exception("Canvas Is Null, Could Be Because The Menu Canvas Has Parent!");
        }
    }





    private void Update()
    {
        if (IsVisible)
        {
            //transform.position = Vector3.Lerp(transform.position, visiblePosition, Time.deltaTime * moveSpeed);
            transform.position = transitionInfo.CalculateNextEnterPosition(transform.position, visiblePosition, canvas);
        } else
        {
            //transform.position = Vector3.Lerp(transform.position, hiddenPosition.position, Time.deltaTime * moveSpeed);
            transform.position = transitionInfo.CalculateNextExitPosition(transform.position, canvas);
        }
    }



    

}




