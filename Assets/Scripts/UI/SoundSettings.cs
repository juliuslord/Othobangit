using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    public AudioSource backgroundMusic;

    public AudioSource generalSound;

    public float thisVolume;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("PushableObject"))
        {
            backgroundMusic.volume = thisVolume;
        }
    }
}
