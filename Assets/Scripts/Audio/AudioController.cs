using System;
using UnityEngine;
using Util;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        public RuntimeVar<MonoBehaviour> audioController;

        // How intense the game is currently
        public float musicalIntensity;

        // How strictly should the intensity of a selected track match the current game intensity?
        public float intensityVariance;

        public SoundData[] musicSounds, sfxSounds;
        public AudioSource musicsSource, sfxSource;

        private void OnEnable() { audioController.Value = this; }

        private void OnDisable() { audioController.Value = null; }

        // This is just for demo/testing and can be removed for prod. 
        private void Start() { PlayMusicIntensity(musicalIntensity); }


        public void PlayMusicIntensity(float intensity)
        {
            // Find sound by intensity in SoundData array
            SoundData s = Array.Find(musicSounds, item => Mathf.Abs(item.intensity - intensity) <= intensityVariance);
            // if sound isn't found within variance range
            if (s == null)
            {
                // Log a warning message
                Debug.LogWarning("Sound with intensity: " + intensity + " not found!");
                return;
            }

            // Otherwise set the audio source's clip to the selected sound's clip and play
            musicsSource.clip = s.clip;
            musicsSource.Play();
        }

        // Plays audio SFX based on its name. 
        public void PlaySFX(string soundName)
        {
            SoundData sound = Array.Find(sfxSounds, x => x.name == soundName);

            if (sound == null) { Debug.Log("Sound Not Found"); } else { sfxSource.PlayOneShot(sound.clip); }
        }
    }
}