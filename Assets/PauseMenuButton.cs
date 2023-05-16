using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Refrences")]
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Image buttonLeftArrow;
    [SerializeField] private Image buttonRightArrow;

    [Header("General Settings")]
    [Tooltip("Will The Button Trigger As Soon As You Press Down")]
    [SerializeField] private bool InstantTrigger;

    [Header("Color")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color pressedColor;

    [Header("Sound")]
    [SerializeField] private AudioClipInfo clickSound;

    [Header("Custom Events")]
    [SerializeField] private UnityEvent _onClick;
    [SerializeField] private UnityEvent _onEnter;
    [SerializeField] private UnityEvent _onExit;

    bool justTriggered = false;
    bool mouseDown = false;
    bool mouseHovering = false;

    private void Update() {
        if(mouseHovering) {
            //buttonText.fontStyle = FontStyles.Underline;
            buttonLeftArrow.gameObject.SetActive(true);
            buttonRightArrow.gameObject.SetActive(true);
        } else {
            //buttonText.fontStyle = FontStyles.Normal;
            buttonLeftArrow.gameObject.SetActive(false);
            buttonRightArrow.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
    }

    private void OnClick() {
        AudioManager.Instance.PlayClip(clickSound, Vector3.zero);
        _onClick.Invoke();
    }

    private void OnEnter() {
        mouseHovering = true;
        buttonText.color = hoverColor;
        _onEnter.Invoke();
    }

    private void OnExit() {
        mouseHovering = false;
        buttonText.color = normalColor;
        _onExit.Invoke();
    }

    // void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    // {
    //     if(!eventData.used && InstantTrigger) {
    //         OnClick();
    //         eventData.Use();
    //     }
    // }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(!eventData.used) {
            OnEnter();
            eventData.Use();
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if(!eventData.used) {
            OnExit();
            eventData.Use();
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(!eventData.used) {
            buttonText.color = pressedColor;
            mouseDown = true;

            if(InstantTrigger) {
                OnClick();
                justTriggered=true;
            }
            eventData.Use();
        }
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if(!eventData.used && !justTriggered) {
            buttonText.color = normalColor;
            mouseDown = false;


            //make sure the mouse is still over the button
            if(mouseHovering) {
                OnClick();
            }

            eventData.Use();
        }
        justTriggered = false;
    }
}
