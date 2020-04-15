using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public static bool objectFollowing = false;
    // Tells CameraController what kind of object is being followed
    public static bool typeOfObjectDefine = false;
    // Allows each object to be defined within the inspector
    public bool isBuilding = false;
    // public GameObject interactionUI;
    private float firstClick, timeBetweenClicks;
    private int clickCounter;
    // private GameObject resetCamera = CameraController.cameraRig;
    private Vector3 moveVector;
    // Displays Animal Information Panel

    void Start () {

        // AnimalAI = this.gameObject.GetComponent<AnimalAI> ();
        // Ties the two bools together so CameraController can access what has been defined in inspector
        typeOfObjectDefine = isBuilding;
        // Defines click counter and timer
        // firstClick = 0f;
        // timeBetweenClicks = 0.2f;
        // clickCounter = 0;

    }
    public void Update () {
        // Removes object as current camera focus
        if (Input.GetMouseButtonDown (1) && objectFollowing == true) {
            // Sets focus position back to the camera rig
            // gameObject.GetComponent<AnimalAI> ().AnimalNeedsResetter ();

            // AnimalNeedsReseter ();
            objectFollowing = false;
            CameraController.instance.target = CameraController.instance.controller.transform;

            CameraController.cameraRigFocus = true;
            GameplayUI.AnimalPanel.SetActive (false);
            // interactionUI.SetActive (true);
        }
        if (CameraController.instance.target == null) {
            objectFollowing = false;
            CameraController.instance.target = CameraController.instance.controller.transform;

            CameraController.cameraRigFocus = true;
            GameplayUI.AnimalPanel.SetActive (false);
        }
    }
    void LateUpdate () {
        // this.gameObject.GetComponent<AnimalAI> ().AnimalInfoPanelResetter ();
        // Debug.Log(this.gameObject);
        if (this.gameObject.layer == 10) {
            gameObject.GetComponent<AnimalAI> ().AnimalInfoPanelUpdater ();
        }
    }
    // When object is clicked the camera focuses
    public void OnMouseDown () {

        if (PlaceObject.objectInHand == false) {
            // Only allows camera focus after a double click
            // clickCounter += 1;
            // if (clickCounter == 1) {
            //     firstClick = Time.time;
            //     StartCoroutine (DoubleClickDetection ());
            // }
            // if (clickCounter >= 2) {
            GameplayUI.AnimalPanel.SetActive (false);

            // interactionUI.SetActive (false);
            // moveVector = transform.position - CameraController.instance.controller.transform.position;
            // CameraController.instance.controller.Move (moveVector * Time.deltaTime);
            // Vector3.Lerp (CameraController.instance.controller.transform.position, moveVector, Time.deltaTime);
            CameraController.instance.controller.transform.position = transform.position;
            CameraController.instance.target = transform;
            CameraController.cameraRigFocus = false;
            objectFollowing = true;
            // clickCounter = 0;
            // If the object taking focus is an animal then activate the animal UI Info Panel and populate it with current animals needs
            if (this.gameObject.layer == 10) {
                // Renders the Animal Info Panel with all stats from current animal
                GameplayUI.AnimalPanel.SetActive (true);
                this.gameObject.GetComponent<AnimalAI> ().AnimalInfoPanelUpdater ();
            }
            // }
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