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

    [ConsoleCommand("no", "plays hangup noise")]
    public string playAudio() {
        Debug.Log("no");
        return "no";
    }

    [ConsoleCommand("hangup", "plays hangup noise")]
    public string hungup() {
        audioSource.PlayOneShot(audioClip);
        Debug.Log("played");
        return "played";
    }

    [ConsoleCommand("loser", "ur loser")]
    public string loser() {
        return "loser";
    }

    [ConsoleCommand("number", "says number")]
    public string num(int n) {
        return "ur Number is " + n;
    }

    [ConsoleCommand("phrase", "says your phrase back")]
    public string phrase(string n) {
        return "ur phrase is " + n;
    }

    [ConsoleCommand("cube", "spawns cube at pos")]
    public string phrase(Vector3 pos) {
        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube),pos,Quaternion.identity);
        return "Spawned cube at " + pos;
    }


}
