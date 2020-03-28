using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    private bool paused = false;
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
        Time.timeScale = 0.0f;
        paused = true;

    }
    public void ResumeGame () {
        paused = false;
        PauseMenu.SetActive (false);
        Time.timeScale = 1.0f;

    }
    public void ExitToMenu () {
        SceneManager.LoadScene (0);
    }
}