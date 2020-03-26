using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {
    protected Transform XForm_Camera;
    protected Transform XForm_Parent;

    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    public float OrbitSpeed = 10f;
    public float ScrollSpeed = 6f;
    public bool CameraDisabled = false;

    protected Vector3 LocalRotation;
    protected float CameraDistance = 10f;

    // Start is called before the first frame update
    void Start () {
        this.XForm_Camera = this.transform;
        this.XForm_Parent = this.transform.parent;
    }

    // LateUpdate is called once per frame, after Update() on every game object in the scene
    void LateUpdate () 
    {
        if (Input.GetKeyDown (KeyCode.LeftShift)) 
        {
            CameraDisabled = !CameraDisabled;
        }

        if (!CameraDisabled) 
        {
            // Rotation of the camera based on mouse coordinates
            if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
                LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                // Clamp the y Rotation to horizon and not flipping over at the top
                LocalRotation.y = Mathf.Clamp(LocalRotation.y, 0f, 90f);
            }
            //Zooming input from our mouse scroll wheel
            if(Input.GetAxis("Mouse ScrollWheel") != 0f){
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

                //Makes camera zoom faster the further away it is from the target
                ScrollAmount *= (this.CameraDistance * 0.3f);

                this.CameraDistance += ScrollAmount * -1f;

                //This makes the camera go no closer than 1.5 meters from target and no further than 100 meters. ***Optional
                this.CameraDistance += Mathf.Clamp(this.CameraDistance, 1.5f, 100f);
            }
        }
        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(LocalRotation.y, LocalRotation.x, 0);
        this.XForm_Parent.rotation = Quaternion.Lerp(this.XForm_Parent.rotation, QT, Time.deltaTime * OrbitSpeed);

        if(this.XForm_Camera.localPosition.z != this.CameraDistance * -1f)
        {
            this.XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.XForm_Camera.localPosition.z, this.CameraDistance * -1f, Time.deltaTime * ScrollSpeed));
        }
    }
}