using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceObject : MonoBehaviour {
    public static bool objectInHand = false;
    public GameObject actualObject;
    public GameObject blueprintObject;

    private GameObject tempObject;
    private float mouseWheelRotation;
    private Quaternion rotationHolder;

    public LayerMask clickMask;
    public Button UIButton;

    private float startTime, endTime;

    void Start () {
        Button btn = UIButton.GetComponent<Button> ();
        btn.onClick.AddListener (HandleNewObjectHotkey);
        startTime = 0f;
        endTime = 0f;
    }
    void Update () {

        // Deselects current object placement
        if (Input.GetMouseButtonDown (1)) {

            startTime = Time.time;
        }
        if (Input.GetMouseButtonUp (1)) {
            endTime = Time.time;

            if (endTime - startTime < .15f) {
                Destroy (tempObject);
                objectInHand = false;

            }
        }

    }
    private void LateUpdate () {

        // HandleNewObjectHotkey ();
        if (tempObject != null) {
            if (MenuButtons.paused == false) {
                RotateFromMouseWheel ();
                MoveCurrentObjectToMouse ();
                ReleaseIfClicked ();

            }
        }

        //        if(Input.GetMouseButtonDown(1)) {
        //         Destroy (temporaryObjectBlueprint);

        // }
    }

    public void HandleNewObjectHotkey () {

        // DestroyImmediate (temporaryObjectBlueprint, true);
        // Prevents placing objects while focused on another
        if (CameraFollow.objectFollowing == false) {

            // If already holding an object destroy it, otherwise create a new one
            if (objectInHand == true) {
                DestroyImmediate (tempObject, true);
                objectInHand = false;
            } else {
                DestroyImmediate (tempObject, true);
                tempObject = Instantiate (blueprintObject);
                objectInHand = true;
            }
        }
    }

    private void MoveCurrentObjectToMouse () {
        // Vector3 clickPosition = -Vector3.one;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast (ray, out hitInfo, 300f, clickMask)) {
            // clickPosition = hitInfo.point;
            tempObject.transform.position = hitInfo.point;
            //prevents object from clipping through ground layer ***NEED THIS
            tempObject.transform.position += new Vector3 (0f, BlueprintPlacement.objectOffsetY, 0f);
            // Allows object being placed to rotate to orientation of terrain -- Might be needed for hills
            if(tempObject.tag == "Animal"){
            tempObject.transform.rotation = Quaternion.FromToRotation (Vector3.up, hitInfo.normal);
            }

        } else {
            DestroyImmediate (tempObject, true);
                objectInHand = false;

        }
    }

    private void RotateFromMouseWheel () {
        // Debug.Log (Input.mouseScrollDelta);
        if (Input.GetKey (KeyCode.N)) {
            // mouseWheelRotation += Input.mouseScrollDelta.y;
            tempObject.transform.Rotate (Vector3.down, 1);
            // rotationHolder = tempObject.transform.rotation;

        }
        if (Input.GetKey (KeyCode.M)) {
            // mouseWheelRotation += Input.mouseScrollDelta.y;
            tempObject.transform.Rotate (Vector3.up, 1);
            // rotationHolder = tempObject.transform.rotation;

        }
    }

    private void ReleaseIfClicked () {

        if (Input.GetMouseButtonDown (0)) {
            // Grabs the current rotation information from the blueprint
            rotationHolder = tempObject.transform.rotation;

            // Only allows object placement if not colliding with other objects
            if (BlueprintPlacement.isColliding == false) {
                // Only allows object placement when not clicking on UI element
                if (!EventSystem.current.IsPointerOverGameObject ()) {
                    // Add logic for decoration and foliage items to be placed as many times as player would like
                    if (BlueprintPlacement.multiPlacedItem == true) {
                        DestroyImmediate (tempObject, true);
                        tempObject = Instantiate (actualObject);
                        tempObject.transform.rotation = rotationHolder;
                        MoveCurrentObjectToMouse ();
                        tempObject = null;
                        objectInHand = false;

                        HandleNewObjectHotkey ();
                        MoveCurrentObjectToMouse ();
                        // DestroyImmediate (tempObject, true);
                        // tempObject = Instantiate (blueprintObject);

                        //  tempObject = Instantiate (blueprintObject);

                    }
                    // Otherwise place an object normally
                    else {
                        //  if(another object is already following mouse)
                        // {
                        // Then delete that object before allowing placement of another
                        // }
                        // Removes Blueprint Object
                        DestroyImmediate (tempObject, true);
                        // Replaces Blueprint with actual object at current position
                        tempObject = Instantiate (actualObject);
                        // Rotates the final placed object to the rotation of blueprint
                        tempObject.transform.rotation = rotationHolder;

                        MoveCurrentObjectToMouse ();

                        tempObject = null;
                        objectInHand = false;
                    }
                }
                
            }
            // If a UI item is click with an object in hand, destroys object
            if (EventSystem.current.IsPointerOverGameObject ()) {
                // Debug.Log ("Ui Clicked");
                DestroyImmediate (tempObject, true);
                tempObject = null;
                objectInHand = false;
            }
            if (BlueprintPlacement.isColliding == true) {
                // Will not allow objects to be placed while overlapped
                Debug.Log ("Can't Place Object Here, Area Is Obstructed");
            }

        }
    }
}