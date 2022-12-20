using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour     // This script is attached to both camera-holding objects so they'll follow the player
{                                               // The grid-player one has rotation speed = 0 so it wont rotate
    public GameObject player;

    public Quaternion targetRot;

    [SerializeField]
    public float rotSpeed;
    private float initialRotSpeed;

    void Start()
    {
        initialRotSpeed = rotSpeed;   // This is to assist holding Shift to increase rotation speed
                                      // Need to reset speed after
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);  // No moving on Y-axis

        FastRotate();
        RotateWithPlayer();
    }

    void RotateWithPlayer()
    {
        if (Input.GetKey(KeyCode.A))    // if press A then rotate right, so the player will face left
        {
            targetRot.eulerAngles = transform.rotation.eulerAngles - new Vector3(0, 1, 0);
        }
        
        else if (Input.GetKey(KeyCode.D))   // if press D then rotate left, so the player will face right
        {
            targetRot.eulerAngles = transform.rotation.eulerAngles + new Vector3(0, 1, 0);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed / 100);   // rotate
    }

    void FastRotate()
    {
        if (Input.GetKey(KeyCode.LeftShift))    // Double rotation speed if holding Left Shift
        {
            rotSpeed = initialRotSpeed * 2;
        }

        else
        {
            rotSpeed = initialRotSpeed;
        }
    }
}
