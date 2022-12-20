using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPad : MonoBehaviour // This script is attached to jump pads and inflicts a vertical force on any box colliding //
{

    public Rigidbody rb;
    private Vector3 pushDirection;
    private Vector3 thrust;

    [SerializeField]
    private float jumpStrength = 2.0f;

    void Start()
    {
        
    }

    void Update()
    {

    }
    /*

    private void OnTriggerStay(Collider other)
    {
        if (other.trigger.CompareTag("PushableObject"))
        {
            var box = other.GetComponent<Rigidbody>();
            if (box != null)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                box.velocity = pushDirection * pushStrength;
            }
        }
    }

    */

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("PushableObject"))
        {
            rb = other.GetComponent<Rigidbody>();

            pushDirection = new Vector3(0, 1, 0);

            thrust = pushDirection * jumpStrength;

            rb.AddForce(0, thrust.y, 0, ForceMode.Impulse);
        }
    }
}