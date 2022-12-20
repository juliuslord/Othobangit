 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGridSnap : MonoBehaviour            // This script is attached to all boxes so they snap to the grid inside the grid section
{
    public Rigidbody rb;

    public Collider boxCollider;

    public GridPlayer guy;

    public bool gridSnapping;

    public bool isIce = false;      // To make ice blocks immune to the change in drag
                            // MUST BE ENABLED FOR ICE

    //[SerializeField]
    //public GameObject sinkHoleCollider;

   // public Vector3 target;
    public bool sinking = false;

    //[SerializeField] public float yLock;

    //public float snapY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        boxCollider = GetComponent<BoxCollider>();

        gridSnapping = false;
    }

    void Update()
    {
        TotalSnap();

        if (gridSnapping)       // if in grid trigger
        {
            if (rb.velocity.magnitude <= 0.2)     // Needed to increase this to 1 for the ice
            {
                //Debug.Log("Box snapping");

                transform.position = new Vector3(
                Mathf.Round(transform.position.x),
                transform.position.y,
                Mathf.Round(transform.position.z));
            }

            /*
            if (rb.velocity.magnitude >= 1)
            {
                guy.Lifted = true;
            }
            */
        }

        /*
        else if (sinking)
        {
            target = transform.position - new Vector3(0, 1, 0);

            var step = 1 * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
        */

        else
        {
            /*
            if (transform.position.y <= -10)            // For sinking, will change it to teleport it later
            {                                           // so it can be reset after teleporting
                gameObject.SetActive(false);    
            }
            */
        }
        
    }

    public void TotalSnap()
    {
         if (guy.boxSnap == true)   // If the grid player is snapping to grid then boxes should snap too
        {
            transform.position = new Vector3(
                Mathf.Round(transform.position.x),
                transform.position.y,
                Mathf.Round(transform.position.z));
            
            Debug.Log("Just now");
        }

         else
        {   }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("JumpableWall"))
        {
            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));

            guy.Lifted = true;

            
            if (isIce == true)
            {
                rb.velocity = new Vector3(0, 0, 0);     // if the box is ice then colliding with a wall sets its velocity to 0
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("JumpableWall"))
        {
            transform.position = new Vector3(       // On exiting a wall, snap to the grid
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));

            guy.Lifted = false;                     // and stop telling the player to snap
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("GridSide"))
        {
            gridSnapping = true;

            transform.rotation = new Quaternion(0, 0, 0, 0);        // Locking rotation

            //rb.constraints = RigidbodyConstraints.FreezePositionY;    // this locks them to the same Y on their Rigid

            rb.constraints = RigidbodyConstraints.FreezeRotation;

           


          //  transform.position = (transform.position.x, 0.6, transform.position.z);
            
            if (isIce)
            {
                rb.mass = 0;
                rb.drag = 0;
            }

            else 
            {
                rb.mass = 500;  // On staying in the grid trigger, the boxes drag and mass are set to higher values
                rb.drag = 500;  // This helps their physics work with being pushed
            }
        

        }

        if (other.gameObject.CompareTag("Sinkhole"))    // On entering sinkhole the box falls through and the hole disappears
        {
            gridSnapping = false;

            sinking = true;

            boxCollider.enabled = !boxCollider.enabled;         // Disables the box collider so the box falls through the floor

            other.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GridSide"))
        {
            gridSnapping = false;

            rb.mass = 1;    // On exiting the grid trigger their values are set to normal-er
            rb.drag = 1;
        }
    }
}
