using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public void AdjustVolume (float newVolume) {
     AudioListener.volume = newVolume;
    }
}
