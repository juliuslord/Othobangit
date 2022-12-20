using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : MonoBehaviour        // The script attached to all humanoid guards //
{                                               // It sets them patrolling to preset points
    
    public Transform[] patrolPoints;

    private int patrolPointIndex;
    private float dist;

    [SerializeField]
    public PlayerController playerController;
    public Transform target;

    public bool alerted;

    private Light lightSource;

    public float relativeStealth;

    Color lerpedColor;

    [SerializeField]
    public AudioSource searchingSound;
    public AudioSource alertedSound;

    private bool searchMusik = false;
    private bool alertMusik = false;
    private bool test = false;
    private bool test2 = false;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        alerted = false;
        patrolPointIndex = 0;  // at the start
        transform.LookAt(patrolPoints[patrolPointIndex].position);    // They look where they're going

        lightSource = GetComponentInChildren<Light>();

        lerpedColor = Color.yellow; // Starting colour turns red upon being spotted, yellow is visible and classic

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        relativeStealth = playerController.stealthColour;   // This is a 0-1 value from the player's stealth health
        
        PlayerSpotted();

        GuardControl();

        AlertSounds();

        navMeshAgent.destination = target.position;
    }

   
    // Patrol
    void Patrol()
    {
        dist = Vector3.Distance(transform.position, patrolPoints[patrolPointIndex].position);

        if (dist <= 0.5f)   // If distance to target waypoint is small enough, go to next waypoint
        {
            IncreaseIndex();
        }

        //Debug.Log(dist);         // For bug fixing
        //Debug.Log(patrolPointIndex);

        navMeshAgent.speed = 5f;    // slow speed for patrolling

        target.position = patrolPoints[patrolPointIndex].position;    // target is current waypoint

    }
 
    void IncreaseIndex()    // Increases waypoint index
    {
        patrolPointIndex++;

        if (patrolPointIndex >= patrolPoints.Length)
        {
            patrolPointIndex = 0;
        }
    }
    //


    // Attack
    void AttackPlayer() // when alerted their target is the player's current location
    {
        navMeshAgent.speed = 10f;       // Double speed for attacking

        target.position = playerController.transform.position;
    }
    //


    void PlayerSpotted()
    {
        if (playerController.spotted == true)   // if player's stealth health reaches zero then bool spotted = true till it regenerates
        {
           // Debug.Log("Enemy sees player is spotted");

            alerted = true;
        }

        
        else if (playerController.spotted == false)
        {
            alerted = false;
        }
        
    }
    
    void GuardControl()
    {
        if (alerted)
        {
            AttackPlayer();
            lightSource.color = Color.red;  // if alerted then red light (it cant be stealth health since that regenerates)
        }

        else if (!alerted)
        {
            Patrol();
            SpottingColour();
            lightSource.color = lerpedColor;
        }
    }

    void SpottingColour()   // linearly moves between yellow and red depending on the 0-1 stealth health taken from the player script
    {
        lerpedColor = Color.Lerp(Color.red, Color.yellow, relativeStealth);
    }

    void AlertSounds()  // Plays searching sound when player is in enemy trigger and alert alarm when guards are alerted
    {
        if (alerted)
        {
            alertMusik = true;
            searchMusik = false;
        }

        else if (!alerted)
        {
            alertMusik = false;
            test = false;
        }

        if (alertMusik == false && playerController.beingSpotted == true)   // beingSpotted is true when the player is in the enemy trigger
        {
            searchMusik = true;
        }

        if (playerController.beingSpotted == false)
        {
            searchMusik = false;
            test2 = false;
        }

        ToggleSounds();
    }

    void ToggleSounds() // Dealing with sounds is a pain with Unity, can't just play a thing and be done
    {
        if (!test)      // Alert
        {
            if (alertMusik)
            {
                alertedSound.Play();
                test = true;
            }

            if (!alertMusik)
            {
                alertedSound.Stop();
            }
        }
        else if (test) { }

        if (!test2)     // Searching
        {
            if (searchMusik)
            {
                searchingSound.Play();
                test2 = true;
            }

            if (!searchMusik)
            {
                searchingSound.Stop();
            }
        }
        else if (test2) { }
    }


    private void OnTriggerStay(Collider other)      // In order to fix guards breaking on a Reset,
    {                                               // I created a trigger at their spawn so, when they pass over it
        if (other.gameObject.CompareTag("Respawn")) // their waypoint index is reset, so they'll go for the first waypoint
        {                                           // it's part of their prefab
           // Debug.Log("Fixed?");                  // Not sure if it's still needed but it doesnt hurt to have it anyways

            patrolPointIndex = 1;
        }
    }
    
}
