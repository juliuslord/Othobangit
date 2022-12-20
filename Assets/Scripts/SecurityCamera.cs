using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour     // This script is attached to all security cameras and controls their lights/rotation
{                                               // The lights/spotting of player are identical to guards/sharks
                                                // Only the rotation is unique
    public Quaternion targetRot;

    [SerializeField]
    public float rotSpeed;

    public PlayerController playerController;
    public Light lightSource;

    public bool alerted;

    public float relativeStealth;

    Color lerpedColor;

    [SerializeField]
    public AudioSource searchingSound;
    public AudioSource alertedSound;

    private bool searchMusik = false;
    private bool alertMusik = false;
    private bool test = false;
    private bool test2 = false;

    void Start()
    {
        targetRot.eulerAngles = new Vector3(0, 90, 0);

        alerted = false;

        lightSource = GetComponentInChildren<Light>();  

        lerpedColor = Color.yellow;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed/100);    // Rotates constantly

        relativeStealth = playerController.stealthColour;   // This goes through the player script as that is where the 'stealth health' is
                                                            // it has to be read by all guards in a level
        SetTarget();

        ConvertingAlert();

        if (alerted)
        {
            lightSource.color = Color.red;

        }

        if (!alerted)
        {
            SpottingColour();
            lightSource.color = lerpedColor;
        }

        AlertSounds();
    }

    void SetTarget()        // This makes them update their angle enough they move linearly
    {
        if (transform.rotation.eulerAngles.y >= (targetRot.eulerAngles.y - 45))
        {
           targetRot.eulerAngles = transform.rotation.eulerAngles + new Vector3(0, 90, 0);
        }
    }

    void ConvertingAlert()  // Handles alert status by connecting to the player script
    {
        if (playerController.spotted == true)
        {
            alerted = true;
        }
        else if (playerController.spotted == false)
        {
            alerted = false;
        }
    }

    void SpottingColour()   // Linearly change colour with the player's stealth health (converted to a range of 0-1)
    {
        lerpedColor = Color.Lerp(Color.red, Color.yellow, relativeStealth);
    }

    void AlertSounds()  // This handles sound control
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

        if (alertMusik == false && playerController.beingSpotted == true)
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

    void ToggleSounds()
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
        else if (test) {  }

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
}
