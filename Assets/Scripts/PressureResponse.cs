using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureResponse : MonoBehaviour   // This script is attached to all moving rocks in the grid area
{                                               // they're bright colours and match with the pressure plates
                                                // This script causes the rocks to move down when their target is reached (all pressure platesd assigned have a box)
    private int lifted = 0;                     // The target is manually set for each rock, there are few so it's convenient

    [SerializeField]
    public int target;          // Target number = number of pressure plates needing pressure, regardless of how many are attached
    public Vector3 moveVector;

    private Vector3 initalPosition;

    public int Lifted { get => lifted; set => lifted = value; }

    private void Start()
    {
        initalPosition = transform.position;        
    }

    void Update()
    {
        //Debug.Log(lifted);        // For bug fixing

        if (Lifted == target)   // if all pressure plates assigned to the rock have things on (assuming I've set the correct value)
        {
            transform.position = Vector3.Lerp(transform.position, initalPosition + moveVector, 1f * Time.deltaTime);      // all rocks have negative moveVector y-values
        }

        else
        {
            transform.position = Vector3.Lerp(transform.position, initalPosition, 5f * Time.deltaTime);
        }
    }


}
