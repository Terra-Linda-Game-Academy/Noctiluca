using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;


[Serializable]
public class PausePageInfo {
    public string pageName;
    public int pageID;
    public GameObject pageObject;
}

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuCanvas;
    public float bookOpenCloseTime = 0.1f;
    bool inOpenCloseTransition = false;

    public BookOpenTransition BookOpenCloseTransition;

    public int currentPageID = 0;

    private GameObject currentPageObject;

    [SerializeField] private PausePageInfo[] pausePages;

    [SerializeField] private AudioClipInfo closeBookSound;

    [SerializeField] private AudioClipInfo[] pageFlipSounds;

    private bool isOpen = false;





    public void Start() {
        pauseMenuCanvas = transform.GetComponentInChildren<Canvas>(true).gameObject;
        pauseMenuCanvas.SetActive(false);

        foreach(PausePageInfo pageInfo in pausePages) {
            pageInfo.pageObject.SetActive(false);
        }
    }

    public void Update() {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !inOpenCloseTransition) {
            if(pauseMenuCanvas.activeSelf) {
                ClosePauseMenu();
            } else {
                OpenPauseMenu();
            }
        }
    }

    public void OpenPage(int pageID) {
        if(currentPageObject != null) {
            currentPageObject.SetActive(false);
            AudioManager.Instance.PlayClip(pageFlipSounds[UnityEngine.Random.Range(0, pageFlipSounds.Length)], Vector3.zero);
        }
            

        
        currentPageID = pageID;
        currentPageObject = GetPageObject(pageID);
        currentPageObject.SetActive(true);
        

    }
    private GameObject GetPageObject(int pageID) {
        foreach(PausePageInfo pageInfo in pausePages) {
            if(pageInfo.pageID == pageID) {
                return pageInfo.pageObject;
            }
        }
        return null;
    }

    private void ClosePauseMenu() {
        StartCoroutine(C_ClosePauseMenu());
    }

    private IEnumerator C_ClosePauseMenu() {
        inOpenCloseTransition = true;

        if(currentPageObject != null) {
            for (int i = 8; i > 0; i--)
            {
                currentPageObject.transform.localScale = new Vector3(0.2f + (i * 0.1f), 1f, 1f);
                yield return new WaitForSeconds(0.01f);
                
            }
            currentPageObject.SetActive(false);
        }
        AudioManager.Instance.PlayClip(closeBookSound, Vector3.zero);
            
        BookOpenCloseTransition.Close(bookOpenCloseTime);
        yield return new WaitForSeconds(bookOpenCloseTime);
        isOpen = false;
        pauseMenuCanvas.SetActive(false);
        inOpenCloseTransition = false;
        
    }

    public void OpenPauseMenu() {
        StartCoroutine(C_OpenPauseMenu());
    }

    public IEnumerator C_OpenPauseMenu() {
        inOpenCloseTransition = true;
        pauseMenuCanvas.SetActive(true);
        if(currentPageObject != null)
            currentPageObject.SetActive(false);
        BookOpenCloseTransition.Open(bookOpenCloseTime);
        yield return new WaitForSeconds(bookOpenCloseTime);
        isOpen = true;
        OpenPage(0);
        for (int i = 0; i < 8; i++)
        {
            currentPageObject.transform.localScale = new Vector3(0.2f + (i * 0.1f), 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }
        inOpenCloseTransition = false;
    }

    public void Resume() {
        ClosePauseMenu();
    }


    public void Quit() {
        Application.Quit();
    }
}
