using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceObject : MonoBehaviour {
    [SerializeField]
    private GameObject placeableObjectPrefab;

    [SerializeField]
    // private KeyCode newObjectHotkey = KeyCode.A;

    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;

    public LayerMask clickMask; 
    public Button UIButton;


 void Start(){
    Button btn = UIButton.GetComponent<Button>();
        btn.onClick.AddListener(HandleNewObjectHotkey);
}

    private void Update () {
        // HandleNewObjectHotkey ();

        if (currentPlaceableObject != null) {
            MoveCurrentObjectToMouse ();
            // RotateFromMouseWheel ();
            ReleaseIfClicked ();
        }

               if(Input.GetMouseButtonDown(1)) {
                Destroy (currentPlaceableObject);

        }
    }

    public void HandleNewObjectHotkey () {
        // if (Input.GetKeyDown (newObjectHotkey)) {
            if (currentPlaceableObject != null) {
                Destroy (currentPlaceableObject);
            } else {
                currentPlaceableObject = Instantiate (placeableObjectPrefab);
            }
        // }

        if(Input.GetMouseButtonDown(1)) {
                Destroy (currentPlaceableObject);

        }
    }

    private void MoveCurrentObjectToMouse () {
        // Vector3 clickPosition = -Vector3.one;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast (ray, out hitInfo, 300f, clickMask)) {
                // clickPosition = hitInfo.point;
                currentPlaceableObject.transform.position = hitInfo.point;
                //prevents object from clippiung through ground layer ***NEED THIS
                currentPlaceableObject.transform.position +=  new Vector3(0f, .5f, 0f);
                currentPlaceableObject.transform.rotation = Quaternion.FromToRotation (Vector3.up, hitInfo.normal);
            
        }
        else{
            Destroy (currentPlaceableObject);
        }
    }

    // private void RotateFromMouseWheel () {
    //     Debug.Log (Input.mouseScrollDelta);
    //     mouseWheelRotation += Input.mouseScrollDelta.y;
    //     currentPlaceableObject.transform.Rotate (Vector3.up, mouseWheelRotation * 10f);
    // }

    private void ReleaseIfClicked () {
        if (Input.GetMouseButtonDown (0)) {
            currentPlaceableObject = null;
        }
    }
}