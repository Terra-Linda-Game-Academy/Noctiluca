using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private List<AudioClipInfo> playedMusic = new List<AudioClipInfo>();
    public List<AudioClipInfo> music = new List<AudioClipInfo>();

    public AudioObjectController currentSongObject;

    public static MusicController Instance;

    // Start is called before the first frame update
    private void PlaySong()
    {
        if(music.Count == playedMusic.Count)
            playedMusic.Clear();

        List<AudioClipInfo> possibleMusic = new List<AudioClipInfo>();;
        possibleMusic.AddRange(music);
        possibleMusic.RemoveAll(x => playedMusic.Contains(x));


        AudioClipInfo selectedTrack = possibleMusic[Random.Range(0, possibleMusic.Count)];
        playedMusic.Add(selectedTrack);
        //Debug.Log(selectedTrack.audioClip.name);
        currentSongObject = AudioManager.Instance.PlayClip(selectedTrack, Vector3.zero, OnSongEnd);
        //selectedTrack.CreateBasicInstance(Vector3.zero);
    }

    private void Start() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
        PlaySong();
    }

    private void OnSongEnd(AudioObjectController audioObjectController)
    {
        PlaySong();
    }
}
