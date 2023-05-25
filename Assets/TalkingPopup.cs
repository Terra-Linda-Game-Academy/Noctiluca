using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;

public class TalkingPopup : MonoBehaviour
{
    public Animator[] animators;
    public TextMeshProUGUI dialogText;

    int tick = 0;
    private void Open()
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger("Open");
        }
    }

    private void Close()
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger("Close");
        }
    }

    
    IEnumerator DialogPopup(string text, float timeBetween) {
        dialogText.text = "";
        Open();
        yield return new WaitForSeconds(1f);
        foreach (var letter in text)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(timeBetween);
        }
        yield return new WaitForSeconds(5f);
        Close();

    }

    public void StartDialog(string text, float timeBetween)
    {
        StartCoroutine(DialogPopup(text, timeBetween));
    }

}
