using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour {

public GameObject interactionUI;
    private float firstClick, timeBetweenClicks;
    private int clickCounter;
    // private GameObject resetCamera = CameraController.cameraRig;

    void Start () {
        firstClick = 0f;
        timeBetweenClicks = 0.2f;
        clickCounter = 0;
    }
    void LateUpdate () {
        // Removes object as current camera focus
        if (Input.GetKeyDown (KeyCode.Space)) {

            // CameraController.instance.target = transform;
            CameraController.cameraRigFocus = true;
            // CameraController.instance.target = CameraController.cameraMain.target;
            // interactionUI.SetActive (true);

            // CameraController.instance.target = resetCamera;

            // CameraController.instance.target = 
        }
    }
    // When object is clicked the camera focuses
    public void OnMouseDown () {
        // Only allows camera focus after a double click
        clickCounter += 1;
        if (clickCounter == 1) {
            firstClick = Time.time;
            StartCoroutine (DoubleClickDetection ());
        }
        if (clickCounter >= 2) {
            // interactionUI.SetActive (false);
            CameraController.instance.target = transform;
            CameraController.cameraRigFocus = false;

            clickCounter = 0;
        }
    }

    public IEnumerator DoubleClickDetection () {
        while (Time.time < firstClick + timeBetweenClicks) {
            yield return new WaitForEndOfFrame ();
        }
        clickCounter = 0;
        firstClick = 0f;
    }
}