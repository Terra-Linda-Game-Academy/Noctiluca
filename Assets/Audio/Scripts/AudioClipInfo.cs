using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Clip Info", menuName = "Audio/AudioClipInfo", order = 1)]
public class AudioClipInfo : ScriptableObject
{
    public AudioClip audioClip;
    public bool loop;

    [Tooltip("-1 Can be heard anywhere")]
    public float range = 10f;
}
