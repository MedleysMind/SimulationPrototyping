using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceObject : MonoBehaviour {
    [SerializeField]
    private GameObject actualObject;

    [SerializeField]
    public GameObject temporaryObjectBlueprint;
    // private KeyCode newObjectHotkey = KeyCode.A;


    private float mouseWheelRotation;

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
                Destroy (temporaryObjectBlueprint);
            }
        }

    }
    private void LateUpdate () {
        // HandleNewObjectHotkey ();
        if (temporaryObjectBlueprint != null) {
            if (MenuButtons.paused == false) {
                MoveCurrentObjectToMouse ();
                // RotateFromMouseWheel ();
                ReleaseIfClicked ();

            }
        }

        //        if(Input.GetMouseButtonDown(1)) {
        //         Destroy (temporaryObjectBlueprint);

        // }
    }

    public void HandleNewObjectHotkey () {

        // if (Input.GetKeyDown (newObjectHotkey)) {
        if (temporaryObjectBlueprint = null) {
            DestroyImmediate (temporaryObjectBlueprint, true);
        } else {
            temporaryObjectBlueprint = Instantiate (actualObject);
        }
        // }

    }

    private void MoveCurrentObjectToMouse () {
        // Vector3 clickPosition = -Vector3.one;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast (ray, out hitInfo, 300f, clickMask)) {
            // clickPosition = hitInfo.point;
            temporaryObjectBlueprint.transform.position = hitInfo.point;
            //prevents object from clippiung through ground layer ***NEED THIS
            temporaryObjectBlueprint.transform.position += new Vector3 (0f, .5f, 0f);
            temporaryObjectBlueprint.transform.rotation = Quaternion.FromToRotation (Vector3.up, hitInfo.normal);

        } else {
            DestroyImmediate (temporaryObjectBlueprint, true);
        }
    }

    // private void RotateFromMouseWheel () {
    //     Debug.Log (Input.mouseScrollDelta);
    //     mouseWheelRotation += Input.mouseScrollDelta.y;
    //     currentPlaceableObject.transform.Rotate (Vector3.up, mouseWheelRotation * 10f);
    // }
//  public void OnPointerClick(PointerEventData data) {
//          // This will only execute if the objects collider was the first hit by the click's raycast
//          Debug.Log(": I was clicked!");
//      }
    private void ReleaseIfClicked () {
        
        if (Input.GetMouseButtonDown (0)) {
            // Only allows object placement when not clicking on UI element
            if (!EventSystem.current.IsPointerOverGameObject())
             {
                //  if(another object is already following mouse)
                // {
                    // Then delete that object before allowing placement of another
                // }
            temporaryObjectBlueprint = null;
             }
        }
    }
}