using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


[Serializable]
public class PausePageInfo {
    public string pageName;
    public int pageID;
    public GameObject pageObject;
}

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuCanvas;

    public int currentPageID = 0;

    private GameObject currentPageObject;

    [SerializeField] private PausePageInfo[] pausePages;

    [SerializeField] private AudioClipInfo[] pageFlipSounds;

    private bool isOpen = false;

    public void Start() {
        pauseMenuCanvas = transform.GetComponentInChildren<Canvas>(true).gameObject;
        pauseMenuCanvas.SetActive(false);
    }

    public void Update() {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Escape)) {
            if(pauseMenuCanvas.activeSelf) {
                ClosePauseMenu();
            } else {
                OpenPauseMenu();
            }
        }

        if(isOpen) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
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
        pauseMenuCanvas.SetActive(false);
        isOpen = false;
    }

    public void OpenPauseMenu() {
        pauseMenuCanvas.SetActive(true);
        isOpen = true;
        OpenPage(0);
    }

    public void Resume() {
        ClosePauseMenu();
    }


    public void Quit() {
        Application.Quit();
    }
}
