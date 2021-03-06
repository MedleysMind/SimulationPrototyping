﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public static bool paused = false;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start () {

    }

    void Update () {
        // Toggles Pause Menu during gameplay
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (paused == false) {
                PauseGame ();
            } else {
                ResumeGame ();
            }
        }
    }
    // Main Menu button management
    public void NewGame () {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene (1);
    }
    public void LoadGame () {

    }
    public void GameSettings () {

    }
    public void ExitGame () {
        Application.Quit ();
    }

    // In game button management
    public void PauseGame () {
        PauseMenu.SetActive (true);
        paused = true;
        Time.timeScale = 0.0f;

    }
    public void ResumeGame () {
        paused = false;
        PauseMenu.SetActive (false);
        if (WorldClock.worldPaused == false) {
            Time.timeScale = 1.0f;
        } else{ Time.timeScale = 0.0f;}

    }
    public void ExitToMenu () {
        SceneManager.LoadScene (0);
    }
}