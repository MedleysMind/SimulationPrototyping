using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{

    void LateUpdate(){
        if(Input.GetKey(KeyCode.Space)){

    CameraController.instance.target = null;
    // CameraController.instance.Init();
    }
    }
    // When object is clicked the camera focuses
public void OnMouseDown()    {
    CameraController.instance.target = transform;
}
}
