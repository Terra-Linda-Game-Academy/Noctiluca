using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;

public class TalkingPopup : MonoBehaviour
{
    public Animator[] animators;
    public TextMeshProUGUI dialogText;

    int currentDialogIdentifier = 0;


    bool open = false;
    private void Open()
    {
        if(!open) {
            foreach (var animator in animators)
            {
                animator.SetTrigger("Open");
            }
            open = true;

        }
        
    }

    private void Close()
    {
        if(open) {
            foreach (var animator in animators)
            {
                animator.SetTrigger("Close");
            }
            open = false;
             
        }
    }

    
    IEnumerator DialogPopup(string text, float timeBetween) {
        int identifier = currentDialogIdentifier;
        dialogText.text = "";
        Open();
        yield return new WaitForSeconds(1f);
        foreach (var letter in text)
        {
            //Makes sure same dialog
            if(identifier != currentDialogIdentifier) {
                yield break;
            }
            dialogText.text += letter;
            yield return new WaitForSeconds(timeBetween);
        }
        
        yield return new WaitForSeconds(5f);

        //Makes sure same dialog
        if(identifier != currentDialogIdentifier) {
            yield break;
        }

        Close();

    }

    public void StartDialog(string text, float timeBetween)
    {
        currentDialogIdentifier++;
        StartCoroutine(DialogPopup(text, timeBetween));
    }

}
