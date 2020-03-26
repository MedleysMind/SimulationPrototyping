using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FocusBounder : MonoBehaviour
{

    void Update()
    {
 if (transform.position.y < 1) {
      transform.position = new Vector3(transform.position.x, 1, transform.position.z);
   }
    }
}
