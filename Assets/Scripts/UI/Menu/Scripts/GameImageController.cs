using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameImageController : MonoBehaviour
{
    AudioSource audioSource;

    public int averageSkip = 500;
    public float audioSizeMultiplier = 5f;

    // Update is called once per frame
    void Update()
    {
        audioSource = MusicController.Instance.currentSongObject.audioSource;
        if(audioSource.isPlaying) {
            float[] spectrumData=new float[8192];  
            audioSource.GetSpectrumData(spectrumData,0,FFTWindow.BlackmanHarris);  

            float total = 0f;
            int count = 0;
            for(int i = 0; i < 8192; i+=averageSkip) {
                total+= spectrumData[i];
                count++;
            }
            float spectrumAverageScaled = (total / count) * audioSizeMultiplier;
            float size = spectrumAverageScaled + 1f;
            transform.localScale = new Vector3(size, size, size);
            transform.localScale = new Vector3(size, size, size);

            // if(spectrumAverageScaled > audioBeatThreshold) {
            //     beatCube.SetActive(true);
            // } else {
            //     beatCube.SetActive(false);
            // }
        }
    }
}
