using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour
{
    [SerializeField]
    public bool Attract;
    [SerializeField]
    public bool Repel;
    [SerializeField]
    private float magnetStrength;
    [SerializeField]
    public GameObject player;

    private Vector3 initialPosition;

    [SerializeField]
    public Transform startMarker;

    public Transform endMarker;

    // Keep a note of the time the movement started.
    private float startTime;
    // Total distance between the markers.
    private float journeyLength;

    private Vector3 pushDirection;

    public Rigidbody rb;

    private Vector3 thrust;

    void Start()
    {
        // initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        startMarker = GetComponent<Transform>();

        player = GameObject.Find("Free Player");

        endMarker = player.GetComponent<Transform>();
    }

    void Update()
    {
        InputtingMagnetism();
    }

    void InputtingMagnetism()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Attract)
            {
                Attract = false;
                Repel = false;
            }

            else
            {
                Attract = true;
                Repel = false;

                Debug.Log("Attracting");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Repel)
            {
                Attract = false;
                Repel = false;
            }

            else
            {
                Attract = false;
                Repel = true;

                Debug.Log("Repelling");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("An object entered.");
       // Debug.Log("Name of the object: " + other.gameObject.name);

        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

        if (Attract == true)
        {

        }

        else if (Repel == true)
        {
            

            
        }

        else { }
    }

    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("An object is still inside of the trigger");

        Vector3 relative;
        

        pushDirection = (startMarker.position - endMarker.position).normalized;     // Normalized to keep constant force regardless of distance

        relative = transform.InverseTransformDirection(pushDirection);

        if (Attract == true)
        {
            /*
            // Distance = time x speed
            float distCovered = (Time.time - startTime) * (magnetStrength/1000);

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            */
            magnetStrength = 20f;

            thrust = -pushDirection * (magnetStrength / 100);

            //Debug.Log(thrust);

            rb.AddForce(thrust.x, thrust.y, thrust.z, ForceMode.Impulse);

        }

        else if (Repel == true)
        {
            /*
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * (magnetStrength / 1000);

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            */

            // Vector3 pushDirection = player.transform.position - transform.position;
            //transform.position = Vector3.Lerp(initialPosition, pushDirection * magnetStrength, 5.0f * Time.deltaTime);

            //gameObject.transform.forward = pushDirection;
            magnetStrength = 25f;

            thrust = pushDirection * (magnetStrength/100);

            rb.AddForce(thrust.x, thrust.y, thrust.z, ForceMode.Impulse);
        }
    }
}
