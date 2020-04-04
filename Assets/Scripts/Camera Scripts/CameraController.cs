using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    //// Public Controllers
    // Used to set what is considered the camera
    public static CameraController instance;
    // public static CameraController cameraMain;
    // Links the Camera Rig to the camera
    public CharacterController controller;
    // Allows the Camera Rig to be set as focus
    public Transform target;
    //// Public Floats
    public float distance = 5.0f;
    public float maxDistance = 150;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public float zoomDampening = 5.0f;
    public float zoomAmount = .5f;
    public float rotationAmount = .25f;
    //// Public Ints
    // Sets the minimum y axis mouse orbit -- 1 Prevents clipping through bottom of map
    public int yMinLimit = 1;
    // Sets the maximum y axis mouse orbit-- 70 Pevents odd camera functionality 
    public int yMaxLimit = 70;
    public int zoomRate = 40;
    //// Public Bools
    public bool edgeScroll = true;
    public bool mousePan = true;
    public static bool cameraRigFocus = true;

    // Layer Selection
    public LayerMask collisionLayers;
    // Private Bools
    private bool CollisionDetected = false;

    //// Private Floats
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    // Used as a base float in many functions
    private float speed = 100;
    private float panSpeed;
    private float currentDistance;
    private float desiredDistance;
    //// Private Quaternions
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    //// Private Vectors
    // private Vector3 targetOffset;
    private Vector3 position;
    private Vector3 moveDirection;

    void Start () {
        Init ();
        // Sets the current focus as this camera rig, allows for focus on specific game objects
        instance = this;
    }
    void OnEnable () { Init (); }

    public void Init () {
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
//If object within certain distance from raycast, move camera position up, back, forward, left and right just enough to offset collision

    public void Update () {
        // Rotation logic on Update allows for smooth rotation
        RotationLogic ();
        // If camera is locked onto an object
        if (ObjectFollow.objectFollowing == true) {
            // If the camera is following a building, set max and min zoom to avoid clipping
            if (ObjectFollow.typeOfObjectDefine == true) {
                maxDistance = 75;
                minDistance = 15;
            }
            // If the object being followed is not a building, set max and min zoom
            else {
                maxDistance = 20;
                minDistance = 5;
            }
        }
        // If not following an object, set max and min zoom distance back to original parameters
        if (ObjectFollow.objectFollowing == false) {
            maxDistance = 150;
            minDistance = 2;
        }
        // Only allows input on camera rig if it is the object in focus
        if (cameraRigFocus && CollisionDetected == false) {
            // Adjusts mouse pan speed depending on camera height
            if (this.position.y < 10) {
                panSpeed = speed * 1.5f;
            } else if (this.position.y < 25 && this.position.y > 10) {
                panSpeed = speed * 2.5f;
            } else if (this.position.y > 25 && this.position.y < 50) {
                panSpeed = speed * 3.5f;

            } else if (this.position.y > 50) {
                panSpeed = speed * 4f;

            }
            // Allows player to turn off in settings
            if (!mousePan) {
                // Debug.Log(mousePan);
                // Mouse pan movement logic
                if (Input.GetMouseButton (1)) {
                    if (PlaceObject.objectInHand == false) {
                        Cursor.lockState = CursorLockMode.Locked;

                    }

                    // transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed/5, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed/5, 0);
                    //grab the rotation of the camera so we can move in a psuedo local XY space
                    // target.rotation = transform.rotation;
                    // target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * speed/5, target);
                    // target.Translate(transform.forward * -Input.GetAxis("Mouse Y") * speed/5);
                    moveDirection = (transform.forward * -Input.GetAxis ("Mouse Y") * panSpeed) + (transform.right * -Input.GetAxis ("Mouse X") * panSpeed);
                    controller.Move (moveDirection * Time.deltaTime);

                } else {
                    Cursor.lockState = CursorLockMode.None;

                }
            }
            // Allows player to turn off in settings
            if (!edgeScroll) {
                // Edge of screen movement scroll
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
        // if(CollisionDetected = true)
        // {

        // }
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate () {

        // Only allows input on camera rig if it is the object in focus
        if (cameraRigFocus) {
            // WASD directional movement on camera rig controller
            moveDirection = (transform.forward * Input.GetAxis ("Vertical") * panSpeed / 4) + (transform.right * Input.GetAxis ("Horizontal") * panSpeed / 4);
            controller.Move (moveDirection * Time.deltaTime);
            // Keeps camera rig from moving to high -- this will be adjusted to whatever is considered sea level
            if (controller.transform.position.y > 1) {
                controller.transform.position = new Vector3 (controller.transform.position.x, 1, controller.transform.position.z);
            }
            // Keeps camera rig from moving to low -- this will be adjusted to whatever is considered sea level
            if (controller.transform.position.y < 1) {
                controller.transform.position = new Vector3 (controller.transform.position.x, 1, controller.transform.position.z);
            }
        }

        // If middle mouse is selected, ORBIT ROTATION
        if (Input.GetMouseButton (2)) {

            xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.03f;
            yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.03f;

            ////////OrbitAngle

            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
            // Sets camera rotation 
            RotationLogic ();
        }

        ///// Rotates the camera usine KEY INPUTS
        // Left and right rotations
        if (Input.GetKey (KeyCode.Q)) {
            //uses original mouse rotation but replaces mouse input with rotationAmount variable
            xDeg += rotationAmount * xSpeed * 0.03f;
            // Sets camera rotation 
            RotationLogic ();
        }
        if (Input.GetKey (KeyCode.E)) {
            // replaces mouse input with rotationAmount variable
            xDeg -= rotationAmount * xSpeed * 0.03f;
            // Sets camera rotation 
            RotationLogic ();
        }
        // Up and down rotations
        if (Input.GetKey (KeyCode.R)) {
            //uses original mouse rotation but replaces mouse input with rotationAmount variable
            yDeg += rotationAmount * ySpeed * 0.02f;
            yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
            // Sets camera rotation 
            RotationLogic ();
        }
        if (Input.GetKey (KeyCode.F)) {
            // replaces mouse input with rotationAmount variable
            yDeg -= rotationAmount * ySpeed * 0.02f;
            yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
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

        ////////Orbit Position
        // Pevents zooming while mouse is over UI elements
        if (!EventSystem.current.IsPointerOverGameObject ()) {
            // affect the desired Zoom distance if we roll the scrollwheel
            desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance);
        }
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp (currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        // calculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * currentDistance);
        // position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
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



    //     void OnCollisionEnter (Collision collisionLayers) {
    //     Debug.Log ("Clipping in Ground");
    //     // position = transform.position;
    //     // desiredDistance -= 1f * Time.deltaTime * Mathf.Abs (desiredDistance);

    //     // controller.Move(moveDirection * 0);
    //     CollisionDetected = true;
    // }
    // void OnCollisionExit (Collision collisionLayers) {
    //     // desiredDistance += 1f * Time.deltaTime * Mathf.Abs (desiredDistance);

    //     // desiredDistance -= 5.5f * Time.deltaTime * Mathf.Abs (desiredDistance);
    //     CollisionDetected = false;
    // }
}