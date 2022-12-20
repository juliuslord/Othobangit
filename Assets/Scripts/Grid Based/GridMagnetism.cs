using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMagnetism : MonoBehaviour
{
    [SerializeField]
    public bool Attract;
    [SerializeField]
    public bool Repel;
    [SerializeField]
    private float magnetStrength = 2.0f;

    private float jumpStrength = 2.0f;

    private Vector3 pushDirection;

    private Vector3 upDirection;

    public Rigidbody rb;

    private Vector3 thrust;

    void Start()
    {
        // initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //  Debug.Log(transform.rotation);

        InputtingMagnetism();
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("An object entered.");
       // Debug.Log("Name of the object: " + other.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {                                                   // Tags used to identify what force should be applied

        if (other.gameObject.CompareTag("MagneticField"))   // Magnetic field
        {
           // Debug.Log("Inside magnetic field");

            // Debug.Log(other.gameObject.transform.position);

            pushDirection = transform.position - other.gameObject.transform.position;


            if (Attract == true)
            {
                thrust = -pushDirection * (magnetStrength / 100);

                rb.AddForce(thrust.x, thrust.y, thrust.z, ForceMode.Impulse);
            }

            else if (Repel == true)
            {
                thrust = pushDirection * (magnetStrength / 100);

                rb.AddForce(thrust.x, thrust.y, thrust.z, ForceMode.Impulse);
            }
        }

        if (other.gameObject.CompareTag("JumpPad"))
        {
            pushDirection = new Vector3(0, 1, 0);

            thrust = pushDirection * jumpStrength;

            rb.AddForce(0, thrust.y, 0, ForceMode.Impulse);
        }
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

                Debug.Log("Grid Attracting");
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

                Debug.Log("Grid Repelling");
            }
        }
    }
}
