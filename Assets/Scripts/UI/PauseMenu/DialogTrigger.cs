using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{

    public string text;
    public float timeBetween = 0.1f;

    public GameUIController gameUIController;

    public Sprite characterImage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameUIController.ShowPopup(text, timeBetween, characterImage);
        }
    }
}
