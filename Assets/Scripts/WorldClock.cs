using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldClock : MonoBehaviour {
    public static bool worldPaused = false;
    public Text timerLabel;
    private float time;

    void Start () { }
    void Update () {
        time += Time.deltaTime;

        var minutes = time / 120; //Divide the guiTime by sixty to get the minutes.
        var seconds = time % 60; //Use the euclidean division for the seconds.
        var fraction = (time * 100) % 100;
        if (minutes == 12) {
            minutes = 1;
        }
        //update the label value
        timerLabel.text = string.Format ("{0:00} : {1:00}", minutes, seconds);
    }
    public void PauseTime () {
        Time.timeScale = 0.0f;
        worldPaused = true;
    }
    public void StartTime () {
        Time.timeScale = 1.0f;
        worldPaused = false;
    }
    public void FastForwardTimeBy2 () {
        Time.timeScale = 2.0f;

    }
    public void FastForwardTimeBy4 () {
        Time.timeScale = 4.0f;

    }
}