using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlayer : MonoBehaviour     // This script is attached to the grid player and controls them
{                                           // INCOMPLETE
    [SerializeField]
    public float pushStrength = 2.0f;
    
    public Controls playerControls;

    // Adjust the speed for the .
    [SerializeField]
    public float speed = 1.0f;

    // The target (cylinder) position.
    public Transform target;

    public bool isMoving;

    private Vector3 resetPos;
    private Quaternion resetRot;

    private bool leftTrueRightFalse = true;
    public bool LeftTrueRightFalse { get => leftTrueRightFalse; set => leftTrueRightFalse = value; }

    public PlayerController playerController;

    public RewindTime timeRewinder;

    [SerializeField]
    public AudioSource checkpointSound;

    public bool boxSnap;

    public AudioSource pushSound;

    void Awake()
    {
        playerControls = new Controls();    // SEE IF NEED DISABLE

        isMoving = false;

        var gridTarget = GameObject.CreatePrimitive(PrimitiveType.Cylinder);        // For a target it creates a primitive object
        gridTarget.transform.localScale = new Vector3(0.15f, 1.0f, 0.15f);

        Collider m_collider;
        m_collider = gridTarget.GetComponent<Collider>();
        m_collider.enabled = !m_collider.enabled;           // Disables its collider so it has no form

        gridTarget.GetComponent<MeshRenderer>().enabled = false;    // Make it invisible, so it can act as a location value only

        // Grab gridTarget values and place on the target.
        target = gridTarget.transform;
        target.transform.position = transform.position;

        resetPos = transform.position;
        resetRot = transform.rotation;
    }

    private void OnEnable()
    {
        //playerControls.Player.Enable();
    }

    void Update()
    {
        if (LeftTrueRightFalse)             // Grid player is on right, so requires this to be false to be active
        { }                                 // Hence the inactivity

        else
        {

            if (target.position != transform.position)  // If we aren't at the target, we're moving
            {
                isMoving = true;
                //Debug.Log("moving");
            }
            else if (target.position == transform.position) // If we are, we're still
            {
                isMoving = false;
                //Debug.Log("not moving");
            }

            Movement();
            SnapWhenStill();
            BlockGlitchFix();
            ResetPosition();
        }

        if (playerController.dead == true)
        {
            Reset();
        }

        if (timeRewinder.isRewinding == true)
        {
            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));

            target.transform.position = transform.position;
        }
    }

    void Movement()
    {
        var step = speed * Time.deltaTime;

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }

        else
        {

            if (Input.GetKeyDown(KeyCode.W))
            {
                target.position = transform.position + new Vector3(0, 0, 1.0f);

                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);            // Makes the character rotate right way
            }

            else if (Input.GetKeyDown(KeyCode.A))
            {
                target.position = transform.position + new Vector3(-1.0f, 0, 0);

                transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                target.position = transform.position + new Vector3(1.0f, 0, 0);

                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                target.position = transform.position + new Vector3(0, 0, -1.0f);

                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }

            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("PushableObject"))
        {
            pushSound.Play();

            var box = hit.rigidbody;
            if (box != null)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                box.velocity = pushDirection * pushStrength;

                
            }
        }
    }

    private void SetTargetCurrent()
    {
        target.transform.position = transform.position;
    }

    private void SnapWhenStill()
    {
        if(!isMoving)
        {
            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));


        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("JumpableWall"))
        {
            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));

            target.transform.position = transform.position;
        }

        else if (other.gameObject.CompareTag("PushableObject"))
        {
              pushSound.Play();
        }
    }

    private void OnTriggerEnter(Collider other)     // Only trigger is spawnpoint, need to play sound and set respawn when entering
    {
        if (other.gameObject.CompareTag("SpawnPoint"))
        {
            if (other.transform.position == resetPos)   // If checkpoint position == player reset position
            {
                    // then nothing happens
            }

            else        // otherwise if it's a new checkpoint then set a new spawn position and play the sound
            {
                checkpointSound.Play();

                resetPos = other.transform.position;
            }
        }

        
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("JumpableWall"))
        {
            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));     // On colliding with a wall, snap to integer positions

            target.transform.position = transform.position; // set the target to the player's position

            Debug.Log("wall");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sinkhole"))
        {
            transform.position = new Vector3(       // Sinkholes act as walls to the player
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));

            target.transform.position = transform.position;
        }
    }

    private bool lifted = false;
    public bool Lifted { get => lifted; set => lifted = value; }

    public void BlockGlitchFix()        // This function sets a bool boxSnap 
    {
        if (Lifted)
        {
            Debug.Log("It worked!");

            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z));

            target.transform.position = transform.position;

            boxSnap = true;
        }
        else if (Lifted == false)
        {
            boxSnap = false;
        }
    }


    private void ResetPosition()
    {
        if (Input.GetKeyDown(KeyCode.R))    // Reset on R press
        {
            Reset();        
        }
    }

    void Reset()    // Normal reset of position and rotation
    {
        transform.position = resetPos;
        transform.rotation = resetRot;
        target.transform.position = resetPos;
    }
}