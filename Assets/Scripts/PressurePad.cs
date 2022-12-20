using System.Collections;
using System.Collections.Generic;
using UnityEngine;
                                            // This script is attached to all pressure plates 
public class PressurePad : MonoBehaviour    // On having an object enter the collision zone it changes the number value
{                                           // of a variable Lifted in the PressureReponse script
    public PressureResponse wall;               

    private void OnTriggerEnter(Collider other)
    {
        wall.Lifted +=1;                                        // Increase/Decrease by 1 if pressure/not
                                                                // Multiple plates can be attached to 1 door this way
      //  mesh.material.SetColor("_Color", Color.green);
    }

    private void OnTriggerExit(Collider other)
    {
        wall.Lifted -= 1;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("An object is on the pressure plate");
    }
}

// https://www.codinblack.com/colliders-and-triggers-in-unity3d/
// This was helpful for understanding in the beginning