using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour {

    void LateUpdate () {
        // Removes object as current camera focus
        if (Input.GetKey (KeyCode.Space)) {

            CameraController.instance.target = null;
            // CameraController.instance.Init();
        }
    }
    // When object is clicked the camera focuses
    public void OnMouseDown () {
        if(Input.GetMouseButton(0)){

        CameraController.instance.target = transform;
        CameraController.instance.cameraRigFocus = false;
        }
    }
}