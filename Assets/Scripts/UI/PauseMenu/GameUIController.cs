using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI TimerText;

    private float startTime;

    public TalkingPopup talkingPopup;

    public void Start() {
        startTime = UnityEngine.Time.time;
    }

    public void Update() {
        float t = UnityEngine.Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        TimerText.text = minutes + ":" + seconds;
    }

    public void ShowPopup(string text, float timeBetween, Sprite characterImage) {
        talkingPopup.StartDialog(text, timeBetween, characterImage);
    }


}