using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    public LayerMask bumperMask;
    private Vector3 moveDirection;
    public CharacterController controller;


void Start(){
    // controller = this;
}
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

//         RaycastHit hit;
//         RaycastHit hitTwo;
//         // Does the ray intersect any objects excluding the player layer
//         if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 25, bumperMask))
//         {
//             Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
//             if(hit.distance <= 2){
//                         // controller.Move (CameraController.moveDirection * Time.deltaTime);
// Debug.Log("Working");
                
//             }
//         }
        // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 25, bumperMask) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitTwo, 25, bumperMask))
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitTwo.distance, Color.yellow);
        //     if(hit.distance <= 2 && hitTwo.distance <= 2){
                
        //     Debug.Log("Forward Ray Hit");
        //         Debug.Log("Less Than 3 away");

        //         //  moveDirection = (transform.up * 100) + (transform.TransformDirection(Vector3.back) * -Input.GetAxis ("Mouse X") * 100);
        //     controller.Move (-CameraController.moveDirection * Time.deltaTime);


        //     }
        // }
        // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 25, bumperMask))
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
        //     if(hit.distance <= 2){
        //               controller.Move (-CameraController.moveDirection * Time.deltaTime);

        //     }
        // }
        //  if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 25, bumperMask))
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.yellow);
        //     if(hit.distance <= 2){
        //              controller.Move (-CameraController.moveDirection * Time.deltaTime);

        //     }
        // }
        //  if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 25, bumperMask))
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
        //     if(hit.distance <= 2){
        //                 controller.Move (-CameraController.moveDirection * Time.deltaTime);

        //     }
        // }
        // else
        // {
        //     // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
        //     Debug.Log("Did not Hit");
        // }
    }
}
