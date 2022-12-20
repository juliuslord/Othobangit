using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharkControl : MonoBehaviour       // This script is attached to the sharks, they move according to the Nav Mesh
{                                               // It's identical to the GuardScript but without sounds or lights

    public Transform[] waypoints;

    private int waypointIndex;
    private float dist;

    [SerializeField]
    public PlayerController playerController;
    public Transform target;

    public bool alerted;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        alerted = false;
        waypointIndex = 0;

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        PlayerSpotted();

        GuardControl();

        navMeshAgent.destination = target.position;
    }


    // Patrol
    void Patrol()
    {
        dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        if (dist <= 0.5f)
        {
            IncreaseIndex();
        }

        //Debug.Log(dist);         // For bug fixing
        //Debug.Log(waypointIndex);

        target.position = waypoints[waypointIndex].position;

        navMeshAgent.speed = 15f;
    }
     void IncreaseIndex()
    {
        waypointIndex++;

        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
    //


    // Attack
    void AttackPlayer()
    {
        navMeshAgent.speed = 20f;

        target.position = playerController.transform.position;
    }
    //


    void PlayerSpotted()
    {
        if (playerController.sharkFood == true)             // This is taken from the free player, if they're in the water collider
        {                                                   // then they're sharkfood and are attacked by sharks
            // Debug.Log("Enemy sees player is spotted");

            alerted = true;
        }


        else if (playerController.sharkFood == false)
        {
            alerted = false;
        }

    }

    void GuardControl()
    {
        if (alerted)                // If alerted, attack the player, otherwise patrol
        {
            AttackPlayer();
        }

        else if (!alerted)
        {
            Patrol();
        }
    }
}
