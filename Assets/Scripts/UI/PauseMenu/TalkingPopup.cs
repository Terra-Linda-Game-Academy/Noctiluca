using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;
using UnityEngine.UI;

public class TalkingPopup : MonoBehaviour
{
    public Animator[] animators;
    public TextMeshProUGUI dialogText;

    int currentDialogIdentifier = 0;

    public Image characterImage;


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

    
    IEnumerator DialogPopup(string text, float timeBetween, Sprite characterSprite) {
        characterImage.sprite = characterSprite;
        int identifier = currentDialogIdentifier;
        dialogText.text = "";
        if(!open) {
            Open();
            yield return new WaitForSeconds(1f);
        }
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

    public void StartDialog(string text, float timeBetween, Sprite characterSprite)
    {
        currentDialogIdentifier++;
        StartCoroutine(DialogPopup(text, timeBetween, characterSprite));
    }

}
