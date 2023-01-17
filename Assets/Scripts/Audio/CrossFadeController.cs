using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeController : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    [SerializeField]
    private float fadeSpeed = 0.5f;

    private void Start()
    {
        audioSource1.volume = 1;
        audioSource2.volume = 0;
    }

    private void Update()
    {
        audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, 0, fadeSpeed * Time.deltaTime);
        audioSource2.volume = Mathf.MoveTowards(audioSource2.volume, 1, fadeSpeed * Time.deltaTime);
    }

    public void CrossFade(SoundData newTrack)
    {
        audioSource2.clip = newTrack.clip;
        audioSource2.Play();
    }
}

