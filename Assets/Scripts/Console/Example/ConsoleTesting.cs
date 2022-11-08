using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleTesting : MonoBehaviour
{

     private AudioSource audioSource;
    public AudioClip audioClip;

    void Start() {
        audioSource=GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
    }


    [ConsoleCommand("hangup", "plays hangup noise")]
    public string hangup() {
        audioSource.PlayOneShot(audioClip);
        Debug.Log("played");
        return "played";
    }


    [ConsoleCommand("cube", "spawns cube at pos")]
    public string phrase(Vector3 pos) {
        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube),pos,Quaternion.identity);
        return "Spawned cube at " + pos;
    }


}
