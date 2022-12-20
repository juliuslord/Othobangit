using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureDoor : MonoBehaviour
{
    public ToggleDoor door;

    private void OnTriggerEnter(Collider other)
    {
        door.Lifted += 1;                                        // Increase/Decrease by 1 if pressure/not
                                                                 // Multiple plates can be attached to 1 door this way
                                                                 //  mesh.material.SetColor("_Color", Color.green);
    }

    private void OnTriggerExit(Collider other)
    {
        door.Lifted -= 1;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("An object is on the pressure plate");
    }
}
