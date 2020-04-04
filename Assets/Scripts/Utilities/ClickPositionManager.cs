using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPositionManager : MonoBehaviour
{

public LayerMask clickMask;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))        
        {
           Vector3 clickPosition = -Vector3.one;

            //Method 1
            // clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3 (0,0,5f));

            //Method 2
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 300f, clickMask))
        {
            clickPosition = hitInfo.point;
            // currentPlaceableObject.transform.position = hitInfo.point;
            // currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }

            Debug.Log(clickPosition);
        }
    }
}
