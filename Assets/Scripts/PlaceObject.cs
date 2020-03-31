using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceObject : MonoBehaviour {
    [SerializeField]
    private GameObject actualObject;

    [SerializeField]
    // private KeyCode newObjectHotkey = KeyCode.A;

    public GameObject temporaryObjectBlueprint;

    private float mouseWheelRotation;

    public LayerMask clickMask; 
    public Button UIButton;

    private float timer;


 void Start(){
    Button btn = UIButton.GetComponent<Button>();
        btn.onClick.AddListener(HandleNewObjectHotkey);
}

    private void LateUpdate () {
        // HandleNewObjectHotkey ();

        if (temporaryObjectBlueprint != null) {
            MoveCurrentObjectToMouse ();
            // RotateFromMouseWheel ();
            ReleaseIfClicked ();
        }

// Deselects current object placement
  if(Input.GetMouseButtonUp(1) || Input.GetKey(KeyCode.Escape)) {
      // Checks to see if button is pressed quickly to allow for mouse pan while in placement mode
            // timer = Time.time;
            // if(Input.GetMouseButtonUp(1)){
            //     if(Time.time-timer < .5f){
            //     Destroy (temporaryObjectBlueprint);

            //     }
            // } else{

            // }
                Destroy (temporaryObjectBlueprint);
            
        }
        //        if(Input.GetMouseButtonDown(1)) {
        //         Destroy (temporaryObjectBlueprint);

        // }
    }

    public void HandleNewObjectHotkey () {
        // if (Input.GetKeyDown (newObjectHotkey)) {
            if (temporaryObjectBlueprint != null) {
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
                temporaryObjectBlueprint.transform.position +=  new Vector3(0f, .5f, 0f);
                temporaryObjectBlueprint.transform.rotation = Quaternion.FromToRotation (Vector3.up, hitInfo.normal);
            
        }
        else{
            DestroyImmediate (temporaryObjectBlueprint, true);
        }
    }

    // private void RotateFromMouseWheel () {
    //     Debug.Log (Input.mouseScrollDelta);
    //     mouseWheelRotation += Input.mouseScrollDelta.y;
    //     currentPlaceableObject.transform.Rotate (Vector3.up, mouseWheelRotation * 10f);
    // }

    private void ReleaseIfClicked () {
        if (Input.GetMouseButtonDown (0)) {
            temporaryObjectBlueprint = null;
        }
    }
}