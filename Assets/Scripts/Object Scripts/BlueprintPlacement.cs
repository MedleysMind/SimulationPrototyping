using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintPlacement : MonoBehaviour {
  public static float objectOffsetY;
  public float offset =.5f;
  public static bool isColliding = false;
    public static bool multiPlacedItem = false;
public bool isDecorationItem = false;

void Start(){
  multiPlacedItem = isDecorationItem;
  // This allows the offset to be adjust publicly for different objects
  objectOffsetY = offset;
}
  // Update is called once per frame
  void Update () {

  }
  void OnCollisionStay (Collision collision) {
    if (collision.gameObject.name == "Terrain") {
      isColliding = true;
      Debug.Log("Clipping in Ground");
    } 
    // else{
    // isColliding = false;
    // }
    isColliding = true;
    // Debug.Log (isColliding);
  }
  void OnCollisionExit (Collision collision) {
    if (collision.gameObject.name == "Terrain") {
      isColliding = false;
      // Debug.Log("Clipping in Ground");
    } 
    isColliding = false;
    // Debug.Log (isColliding);
  }
}