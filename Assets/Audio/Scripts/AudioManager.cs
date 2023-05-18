using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private List<AudioObjectController> audioObjects = new List<AudioObjectController>();
    public AudioObjectController audioObjectPrefab;

    public static AudioManager Instance = null;
    void Awake()
    {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    private void OnAudioObjectOver(AudioObjectController audioObjectController) {
        audioObjects.Remove(audioObjectController);
        Destroy(audioObjectController.gameObject);
    }

    public AudioObjectController PlayClip(AudioClipInfo audioClipInfo, Vector3 position, Action<AudioObjectController> OnEndCallback = null) {
        if(audioClipInfo == null)
            return null;
        AudioObjectController newAudioObject = Instantiate(audioObjectPrefab, position, Quaternion.identity);
        newAudioObject.Init(audioClipInfo, (audioObjectController) => {
            if(OnEndCallback != null)
                OnEndCallback(audioObjectController);
            OnAudioObjectOver(audioObjectController);
        });
        audioObjects.Add(newAudioObject);
        return newAudioObject;
    }
}
