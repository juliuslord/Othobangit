using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour          // This script is attached to all non-player objects to put them back in place
{                                           // upon the R key, player death or object falling too low
    private Vector3 startPos;
    private Quaternion startRot;

    public Rigidbody rb;
    private Vector3 pushDirection;
    private Vector3 thrust;

    public PlayerController playerController;

    private ParticleSystem resetParticleSystem;

    [SerializeField]
    public AudioSource resetSound;

    void Start()
    {
        startPos = transform.position;      // Set the original position as the reset
        startRot = transform.rotation;      // and the rotation

        rb = GetComponent<Rigidbody>();



        resetSound = GetComponent<AudioSource>();
        resetParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()       // The three situations causing a reset
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FullReset();                // Player inputs reset
        }
        
        if (playerController.dead == true)
        {
            FullReset();                // Player dies
        }

        if (transform.position.y <= -20)            
        {                                           
            FullReset();                // Object falls off island
        }
    }

    void FullReset()
    {
        transform.position = startPos;
        transform.rotation = startRot;

        Debug.Log("Resetting");

        resetParticleSystem.Play();     //  For show

        resetSound.Play();              // For hearing

        rb.velocity = new Vector3(0, 0, 0);
    }
}
