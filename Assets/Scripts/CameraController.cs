using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float zoomDampening = 5.0f;
    public float zoomAmount = .5f;
    public float rotationAmount;

    public float panSpeed = 1000;
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    public float speed = 100;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private Vector3 moveDirection;
    public CharacterController controller;
    public bool edgeScroll = true;
    public bool mousePan = true;
    public bool cameraRigFocus = true;

    void Start () {
        Init ();
        // Sets the current focus as this camera rig, allows for focus on specific game objects
        instance = this;
    }
    void OnEnable () { Init (); }

    public void Init () {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target) {
            GameObject go = new GameObject ("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }
        // Grabs the current distance as starting point
        distance = Vector3.Distance (transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        // Grabs the current rotations as starting points
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle (Vector3.right, transform.right);
        yDeg = Vector3.Angle (Vector3.up, transform.up);
    }

    public void Update () {
        // Only allows input on camera rig if it is the object in focus
        if (!cameraRigFocus) {
            // Mouse pan movement logic
            if (!mousePan) {
                if (Input.GetMouseButton (1)) {
                    // transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed/5, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed/5, 0);
                    //grab the rotation of the camera so we can move in a psuedo local XY space
                    // target.rotation = transform.rotation;
                    // target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * speed/5, target);
                    // target.Translate(transform.forward * -Input.GetAxis("Mouse Y") * speed/5);
                    moveDirection = (transform.forward * -Input.GetAxis ("Mouse Y") * speed * 10f) + (transform.right * -Input.GetAxis ("Mouse X") * speed * 10f);
                    controller.Move (moveDirection * Time.deltaTime);
                }
            }
            // Edge of screen movement scroll
            if (!edgeScroll) {
                float edgeSize = 5f;
                if (Input.mousePosition.x > Screen.width - edgeSize) {
                    moveDirection += (transform.right * speed / 2);
                    controller.Move (moveDirection * Time.deltaTime);
                }
                if (Input.mousePosition.x < edgeSize) {
                    moveDirection -= (transform.right * speed / 2);
                    controller.Move (moveDirection * Time.deltaTime);
                }
                if (Input.mousePosition.y > Screen.height - edgeSize) {
                    moveDirection += (transform.forward * speed / 2);
                    controller.Move (moveDirection * Time.deltaTime);
                }
                if (Input.mousePosition.y < edgeSize) {
                    moveDirection -= (transform.forward * speed / 2);
                    controller.Move (moveDirection * Time.deltaTime);
                }
            }
        }
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate () {
        // Only allows input on camera rig if it is the object in focus
        if (!cameraRigFocus) {
            // WASD directional movement on camera rig controller
            moveDirection = (transform.forward * Input.GetAxis ("Vertical") * speed) + (transform.right * Input.GetAxis ("Horizontal") * speed);
            controller.Move (moveDirection * Time.deltaTime);
            // Keeps camera rig from moving to high
            if (controller.transform.position.y > 1) {
                controller.transform.position = new Vector3 (controller.transform.position.x, 1, controller.transform.position.z);
            }
            // Keeps camera rig from moving to low
            if (controller.transform.position.y < 1) {
                controller.transform.position = new Vector3 (controller.transform.position.x, 1, controller.transform.position.z);
            }
        }

        // If middle mouse is selected, ORBIT ROTATION
        if (Input.GetMouseButton (2)) {
            xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

            ////////OrbitAngle

            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
            // Sets camera rotation 
            RotationLogic ();
        }

        // Rotates the camera using selected keys, has a slight pre-shake when switching directions
        if (Input.GetKey (KeyCode.Q)) {
            //uses original mouse rotation but replaces mouse input with rotationAmount variable
            xDeg += rotationAmount * xSpeed * 0.02f;
            // Sets camera rotation 
            RotationLogic ();
        }
        if (Input.GetKey (KeyCode.E)) {
            // replaces mouse input with rotationAmount variable
            xDeg -= rotationAmount * xSpeed * 0.02f;
            // Sets camera rotation 
            RotationLogic ();
        }

        // Zooms the camera using selected keys
        if (Input.GetKey (KeyCode.Z)) {
            desiredDistance -= zoomAmount * Time.deltaTime * Mathf.Abs (desiredDistance);
        }
        if (Input.GetKey (KeyCode.X)) {
            desiredDistance += zoomAmount * Time.deltaTime * Mathf.Abs (desiredDistance);
        }

        // Deselects whatever object is being followed
        if (Input.GetKey (KeyCode.Space)) {

        }
        ////////Orbit Position

        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp (currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        // calculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;

        // If Control and Alt and Middle button? ZOOM!
        // if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
        // {
        //     desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate*0.125f * Mathf.Abs(desiredDistance);
        // }
        // For Smooth rotation
        // currentRotation = Quaternion.Lerp (currentRotation, desiredRotation, Time.deltaTime * panSpeed);
        // Mouse pan movement logic
        //  if (Input.GetMouseButton(1))
        //         {
        //             transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed/5, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed/5, 0);
        //             //grab the rotation of the camera so we can move in a psuedo local XY space
        //             // target.rotation = transform.rotation;
        //             // target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * speed/5, target);
        //             // target.Translate(transform.forward * -Input.GetAxis("Mouse Y") * speed/5);
        //             moveDirection = ((transform.forward * -Input.GetAxis("Mouse Y") * speed/5) + (transform.right * -Input.GetAxis("Mouse X") * speed/5, target));
        //         }
    }
    // Used to calculate all camera rotations
    public void RotationLogic () {
        desiredRotation = Quaternion.Euler (yDeg, xDeg, 0);
        currentRotation = transform.rotation;
        rotation = Quaternion.Lerp (currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;
    }
    // Required to clamp current angle
    private static float ClampAngle (float angle, float min, float max) {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp (angle, min, max);
    }
}