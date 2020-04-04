using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour {

    public static bool objectFollowing = false;
    // Tells CameraController what kind of object is being followed
    public static bool typeOfObjectDefine = false;
    // Allows each object to be defined within the inspector
    public bool isBuilding = false;
    public GameObject interactionUI;
    private float firstClick, timeBetweenClicks;
    private int clickCounter;
    // private GameObject resetCamera = CameraController.cameraRig;
    private Vector3 moveVector;

    void Start () {
        // Ties the two bools together so CameraController can access what has been defined in inspector
        typeOfObjectDefine = isBuilding;
        // Defines click counter and timer
        firstClick = 0f;
        timeBetweenClicks = 0.2f;
        clickCounter = 0;
    }
    void LateUpdate () {

        // Removes object as current camera focus
        if (Input.GetMouseButtonDown (1) && objectFollowing == true) {
            // Sets focus position back to the camera rig
                CameraController.instance.controller.Move (moveVector * Time.deltaTime);

            CameraController.instance.target = CameraController.instance.controller.transform;
            CameraController.cameraRigFocus = true;
            objectFollowing = false;
            // interactionUI.SetActive (true);

        }
    }
    // When object is clicked the camera focuses
    public void OnMouseDown () {
        if (PlaceObject.objectInHand == false) {
            // Only allows camera focus after a double click
            clickCounter += 1;
            if (clickCounter == 1) {
                firstClick = Time.time;
                StartCoroutine (DoubleClickDetection ());
            }
            if (clickCounter >= 2) {
                // interactionUI.SetActive (false);
                moveVector = transform.position - CameraController.instance.controller.transform.position;
                // moveVector = moveVector * 1;
                CameraController.instance.controller.Move (moveVector * Time.deltaTime);
                //  Vector3.Lerp(CameraController.instance.controller.transform.position, moveVector, Time.deltaTime);
                CameraController.instance.controller.transform.position = transform.position;
                CameraController.instance.target = transform;
                CameraController.cameraRigFocus = false;
                objectFollowing = true;

                clickCounter = 0;
            }
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