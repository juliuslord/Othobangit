using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlocking : MonoBehaviour   // This script is attached to the grid Environment object to assist the player in grid snapping when they're supposed to
{
    public GridPlayer guy;

    void Update()
    {
        
        transform.position = new Vector3(           // This keeps the object in it's place
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PushableObject")) 
        {
            Debug.Log("Collision");

            guy.Lifted = true;      // If a box touches the environment then the grid player snaps to grid
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("PushableObject"))
        {
            guy.Lifted = false;     // On the box exiting the environment collision
        }
    }
}
