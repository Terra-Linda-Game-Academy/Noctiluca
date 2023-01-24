using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;


public class MenuItem : MonoBehaviour
{

    public Vector3 visiblePosition;
    private Canvas canvas;
    //private RectTransform rectTransform;

    //[SerializeField] private Transform hiddenPosition;
    [SerializeField] private MenuItemTransitionInfo enterTransitionInfo;
    [SerializeField] private MenuItemTransitionInfo exitTransitionInfo;

    [SerializeField] private UnityEvent OnBecomeVisible;
    [SerializeField] private UnityEvent OnBecomeHidden;


    private Vector3 exitPosition;
    private Vector3 entrancePosition;

    [SerializeField] private float transitionDuration = 1f;
    private float transitionTimer = 0f;


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

            if(!m_IsVisible && value && hasBeenInitilized)
                transform.localPosition = entrancePosition;

            transitionTimer = 0f;

            m_IsVisible = value; 
        }
    }

    private bool m_IsInstant;
    public bool IsInstant { get { return m_IsInstant; } set { m_IsInstant = value; }}


    private bool hasBeenInitilized = false;



    //Don't Set To Awake, For Some Reason I Breaks the vertain local positions
    private void Start()
    {
        //rectTransform = GetComponent<RectTransform>();
        visiblePosition = transform.localPosition; 
        canvas = transform.root.GetComponent<Canvas>();
        if(canvas == null)
        {
            throw new Exception("Canvas Is Null, Could Be Because The Menu Canvas Has Parent!");
        }

        CalculateExitEntrance();

        hasBeenInitilized = true;
    }

    private void CalculateExitEntrance() {
        entrancePosition = enterTransitionInfo.CalculateExitPosition(visiblePosition, canvas);
        exitPosition = exitTransitionInfo.CalculateExitPosition(visiblePosition, canvas);
    }




    Vector2 oldDimensions;
    private void Update()
    {
        //CalculateExitEntrance is probably slow, so only do it when needed
        Vector2 newDimensions = new Vector2(canvas.pixelRect.width, canvas.pixelRect.height);
        if(oldDimensions != newDimensions) {
            CalculateExitEntrance();
            oldDimensions = new Vector2(canvas.pixelRect.width, canvas.pixelRect.height);
        }
        

        transitionTimer += Time.deltaTime/transitionDuration;
        if(IsInstant) {
            if(IsVisible) {
                transform.localPosition = visiblePosition;
            } else {
                transform.localPosition = entrancePosition;
            }
            
        }
        else if (IsVisible)
        {
            //transform.position = Vector3.Lerp(transform.position, visiblePosition, Time.deltaTime * moveSpeed);
            transform.localPosition = enterTransitionInfo.CalculateNextEnterPosition(transform.localPosition, visiblePosition, entrancePosition, transitionTimer, canvas);
        } else
        {
            //transform.position = Vector3.Lerp(transform.position, hiddenPosition.position, Time.deltaTime * moveSpeed);
            transform.localPosition = exitTransitionInfo.CalculateNextExitPosition(transform.localPosition, visiblePosition, exitPosition, transitionTimer, canvas);
        }
    }



    

}




