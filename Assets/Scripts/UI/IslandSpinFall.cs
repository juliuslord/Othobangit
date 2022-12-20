using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpinFall : MonoBehaviour     // This script is attached to all islands on the main menu, it makes them fall
{
    public float speed = 10.0f;
    public int resetHeight = -70;


    private Vector3 target;
    private Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;   // Saves position for reset

        target = startingPos - new Vector3(0, 200, 0);  // Target is 200 below them, straight down
    }

    void Update()
    {
        transform.Rotate(0, 20 * Time.deltaTime, 0);

        MoveDown();
        ResetPosition();
    }

    void MoveDown()
    {
        var step = speed * Time.deltaTime; // calculate distance to move

        transform.position = Vector3.MoveTowards(transform.position, target, step);     // So the islands move down at linear speed
    }

    void ResetPosition()
    {
        if (transform.position.y <= resetHeight)
        {
            transform.position = startingPos;       // When they leave the camera's vision (Y = -70) they reset to their original position 
        }
    }
}
