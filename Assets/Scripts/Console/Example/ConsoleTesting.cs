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


    [ConsoleCommand("cube", "spawns cube at pos", false, "spawned cube at {pos}")]
    public void cube(Vector3 pos) {
        GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = pos;
    }


}
