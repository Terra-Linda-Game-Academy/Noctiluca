using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioObjectController : MonoBehaviour
{
    public AudioSource audioSource;

    Action<AudioObjectController> OnEnd;

    private AudioClipInfo audioClipInfo;
    
    private bool m_IsPlaying;
    public bool IsPlaying
    {
        get { return m_IsPlaying; }
        set { 
            if(value != m_IsPlaying && !value)
                OnEnd.Invoke(this);
            m_IsPlaying = value; 
        }
    }
    


    private void Play() 
    {
        audioSource.clip = audioClipInfo.audioClip;

        if(audioClipInfo.loop)
            audioSource.loop = true;
        else
            audioSource.loop = false;

        
        audioSource.Play();
    }

    private void ForceEnd()
    {
        audioSource.Stop();
        IsPlaying = false;
    }

    private void Update() {
        if(IsPlaying != audioSource.isPlaying)
            IsPlaying = audioSource.isPlaying;
    }



    //if loop count is -1, clip will loop
    public void Init(AudioClipInfo audioClipInfo, Action<AudioObjectController> OnEnd)
    {
        this.audioClipInfo = audioClipInfo;
        this.OnEnd = OnEnd;
        
        audioSource = GetComponent<AudioSource>();

        gameObject.name = audioClipInfo.name + " - Audio Object";

        if(audioClipInfo.range == -1f) {
            audioSource.spatialBlend = 0f;
        } else {
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.maxDistance = audioClipInfo.range;
        }

        Play();
    } 

    
}
