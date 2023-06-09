using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookOpenTransition : MonoBehaviour
{
    public Image image;

    void Awake() {
        // image = GetComponent<Image>();
        image.enabled = false;
    }

    public void Open (float time) { StartCoroutine(C_Open(time));}

    public IEnumerator C_Open (float time) {
        image.enabled = true;
        Vector3 endScale = transform.localScale;
        Vector3 endPosition = transform.position;

        //move from top of screen back to start pos over 90% of the time
        transform.position = new Vector3(transform.position.x, Screen.height, transform.position.z);
        transform.localScale = new Vector3(0, 0, 0);
        
        for (float t = 0; t < time * 0.9f; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(transform.position, endPosition, t / (time * 0.9f));
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, t / (time * 0.9f));
            yield return null;
        }

        image.enabled = false;
    }

    public void Close (float time) {
        StartCoroutine(C_Close(time));
    }

    //same as open but in reverse
    public IEnumerator C_Close (float time) {
        image.enabled = true;
        Vector3 endScale = transform.localScale;
        Vector3 endPosition = transform.position;

        transform.position = endPosition;
        transform.localScale = endScale;
        
        for (float t = 0; t < time * 0.9f; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Screen.height, transform.position.z), t / (time * 0.9f));
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), t / (time * 0.9f));
            yield return null;
        }

        image.enabled = false;

        transform.position = endPosition;
        transform.localScale = endScale;

    }
}
