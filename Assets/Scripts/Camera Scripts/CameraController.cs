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

    public Rigidbody RigRigidBody;
    //// Public Floats
    public float distance = 5.0f;
    public float maxDistance = 150;
    public float minDistance = 2;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public float zoomDampening = 5.0f;
    public float zoomAmount = .5f;
    public float rotationAmount = .25f;
    //// Public Ints
    // Sets the minimum y axis mouse orbit -- 1 Prevents clipping through bottom of map
    public float yMinLimit = 1;
    // Sets the maximum y axis mouse orbit-- 70 Pevents odd camera functionality 
    public float yMaxLimit = 70;
    public int zoomRate = 40;
    //// Public Bools
    public bool edgeScroll = true;
    public bool mousePan = true;
    public static bool cameraRigFocus = true;

    // Layer Selection
    public LayerMask collisionLayers;
    public LayerMask bumperMask;
    // Private Bools
    private bool CollisionDetected = false;
    private bool oribiting = false;

    //// Private Floats
    private float xDeg = 0.0f, yDeg = 0.0f, speed = 100, panSpeed, currentDistance, desiredDistance;
    //// Private Quaternions
    private Quaternion currentRotation, desiredRotation, rotation;
    //// Private Vectors
    // private Vector3 targetOffset;
    private Vector3 position;
    // private Vector3 currentPosition;
    public static Vector3 moveDirection;

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

    public void Update () {
        if (MenuButtons.paused == false) {
            SetZoomParameters ();
            MousePanLogic ();
            CollisionAvoidanceLogic ();
            CameraRigLogic ();
        }
    }
    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate () {
        if (MenuButtons.paused == false) {
            RotationLogic ();
            OrbitLogic ();
            KeyMovementInputs ();
            CameraZoomLogic ();
        }
    }
    public void OrbitLogic () {
        // If middle mouse is selected, ORBIT ROTATION
        if (Input.GetMouseButton (2)) {
            oribiting = true;
            xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

            ////////OrbitAngle

            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
            // Sets camera rotation 
            RotationLogic ();
        } else {
            oribiting = false;

        }
    }
    public void CameraRigLogic () {
        // Only allows input on camera rig if it is the object in focus
        if (cameraRigFocus) {
            // WASD directional movement on camera rig controller
            // moveDirection = (transform.forward * Input.GetAxis ("Vertical") * panSpeed / 4) + (transform.right * Input.GetAxis ("Horizontal") * panSpeed / 4);
            moveDirection = (transform.forward * Input.GetAxis ("Vertical") * panSpeed / 4) + (transform.right * Input.GetAxis ("Horizontal") * panSpeed / 4);
            controller.Move (moveDirection * Time.unscaledDeltaTime);
        }
        // if (CameraRigManager.onGround == false) {
        //     // Move controller down until it is touching the ground again

        //     // controller.transform.position = new Vector3 (controller.transform.position.x, controller.transform.position.y - panSpeed / 4, controller.transform.position.z);
        //     controller.transform.position = new Vector3 (controller.transform.position.x, controller.transform.position.y - 1, controller.transform.position.z);
        // }
        // Keeps camera rig from moving to low -- this will be adjusted to whatever is considered sea level
        if (controller.transform.position.y < 1) {
            controller.transform.position = new Vector3 (controller.transform.position.x, 1, controller.transform.position.z);
        }
        if (controller.transform.position.y > 1) {
            controller.transform.position = new Vector3 (controller.transform.position.x, 1, controller.transform.position.z);
        }
    }
    public void CameraZoomLogic () {
        // Pevents zooming while mouse is over UI elements
        if (!EventSystem.current.IsPointerOverGameObject ()) {
            // affect the desired Zoom distance if we roll the scrollwheel
            desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.unscaledDeltaTime * zoomRate * Mathf.Abs (desiredDistance);
        }
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp (currentDistance, desiredDistance, Time.unscaledDeltaTime * zoomDampening);
        // calculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * currentDistance);
        // position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }
    public void SetZoomParameters () {
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
    }
    public void MousePanLogic () {
        // Only allows input on camera rig if it is the object in focus
        if (cameraRigFocus) {
            // Adjusts mouse pan speed depending on camera height
            if (this.position.y < 10) {
                panSpeed = speed * .75f;
            } else if (this.position.y < 20 && this.position.y > 10) {
                panSpeed = speed * 1f;
            } else if (this.position.y > 20 && this.position.y < 30) {
                panSpeed = speed * 1.25f;
            } else if (this.position.y > 30 && this.position.y < 40) {
                panSpeed = speed * 1.5f;
            } else if (this.position.y > 40 && this.position.y < 50) {
                panSpeed = speed * 2f;
            } else if (this.position.y > 50) {
                panSpeed = speed * 3f;
            }
            // Allows player to turn off in settings
            if (!mousePan && oribiting == false) {
                // Mouse pan movement logic
                if (Input.GetMouseButton (1)) {
                    if (PlaceObject.objectInHand == false) {
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                    moveDirection = (transform.forward * -Input.GetAxis ("Mouse Y") * panSpeed) + (transform.right * -Input.GetAxis ("Mouse X") * panSpeed);
                    controller.Move (moveDirection * Time.unscaledDeltaTime);
                } else {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            // Allows player to turn off in settings
            if (!edgeScroll) {
                // Edge of screen movement scroll
                float edgeSize = 5f;
                if (Input.mousePosition.x > Screen.width - edgeSize) {
                    moveDirection += (transform.right * panSpeed / 2);
                    controller.Move (moveDirection * Time.unscaledDeltaTime);
                }
                if (Input.mousePosition.x < edgeSize) {
                    moveDirection -= (transform.right * panSpeed / 2);
                    controller.Move (moveDirection * Time.unscaledDeltaTime);
                }
                if (Input.mousePosition.y > Screen.height - edgeSize) {
                    moveDirection += (transform.forward * panSpeed / 2);
                    controller.Move (moveDirection * Time.unscaledDeltaTime);
                }
                if (Input.mousePosition.y < edgeSize) {
                    moveDirection -= (transform.forward * panSpeed / 2);
                    controller.Move (moveDirection * Time.unscaledDeltaTime);
                }
            }
        }
    }
    public void KeyMovementInputs () {
        ///// Rotates the camera usine KEY INPUTS
        // Left and right rotations
        // if (Input.GetKey (KeyCode.W)) {
        //     moveDirection += (transform.forward * panSpeed / 2);
        //             controller.Move (moveDirection * Time.unscaledDeltaTime);
        // }
        // if (Input.GetKey (KeyCode.S)) {
        //     moveDirection -= (transform.forward * panSpeed / 2);
        //             controller.Move (moveDirection * Time.unscaledDeltaTime);
        // }
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
            desiredDistance -= zoomAmount * Time.unscaledDeltaTime * Mathf.Abs (desiredDistance);
        }
        if (Input.GetKey (KeyCode.X)) {
            desiredDistance += zoomAmount * Time.unscaledDeltaTime * Mathf.Abs (desiredDistance);
        }
    }

    /////////////////////// FIX THIS ////////////////////////////////
    public void CollisionAvoidanceLogic () {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.down), out hit, 25, collisionLayers)) {
            Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.down) * hit.distance, Color.green);
            if (hit.distance <= 2) {
                yMinLimit = hit.point.y;
                // yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
                // moveDirection = (transform.forward * -Input.GetAxis ("Vertical") * 0) + (transform.right * -Input.GetAxis ("Horizontal") * panSpeed / 4);
                // desiredRotation.y = hit.point.y;
                // Sets camera rotation 
                RotationLogic ();

            }
            if (hit.distance >= 5) {
                yMinLimit = 2;
            }
        }
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 25, collisionLayers)) {
            Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.forward) * hit.distance, Color.red);
            if (hit.distance < 2) {
                speed = 50;
                // minDistance = hit.distance;
                // zoomDampening = 1000;
                // minDistance = hit.point.y + controller.transform.position.y;

                // desiredDistance = zoomAmount * Time.unscaledDeltaTime * Mathf.Abs (desiredDistance);

                yDeg += rotationAmount * speed * 0.02f;
                yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
                // // Sets camera rotation 
                // controller.Move (-moveDirection * Time.unscaledDeltaTime);
                // moveDirection = (transform.forward * Input.GetAxis ("Vertical") * 0) + (transform.right * Input.GetAxis ("Horizontal") * 0);

                RotationLogic ();
            }
        }
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.back), out hit, 25, collisionLayers)) {
            Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.back) * hit.distance, Color.yellow);
            if (hit.distance <= 2) {

                yDeg += rotationAmount * speed * 0.02f;
                yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
                RotationLogic ();

            }
        }
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.left), out hit, 25, collisionLayers)) {
            Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.left) * hit.distance, Color.yellow);
            if (hit.distance < 2) {

                // Forces camera away from layer
                yDeg += rotationAmount * speed * 0.02f;
                yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
                RotationLogic ();
            }
        }
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.right), out hit, 25, collisionLayers)) {
            Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.right) * hit.distance, Color.yellow);
            if (hit.distance < 2) {

                yDeg += rotationAmount * speed * 0.02f;
                yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
                RotationLogic ();
            }

        }
    }
    public void RotationLogic () {

        desiredRotation = Quaternion.Euler (yDeg, xDeg, 0);
        currentRotation = transform.rotation;
        rotation = Quaternion.Lerp (currentRotation, desiredRotation, Time.unscaledDeltaTime * zoomDampening);
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