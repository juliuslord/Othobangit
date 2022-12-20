using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour     // This script is attached to the main camera and follows the correct player
{                                       // as well as handling movement in the intro
    private Transform lookAtTarget;
    public Transform playerLeft;
    public Transform playerRight;

    // Free
    public Transform cameraPosition1;
    public Transform cameraPosition2;
    public Transform cameraPosition3;
    public Transform cameraPosition4;

    // Grid
    public Transform cameraPosition5;
    public Transform cameraPosition6;
    public Transform cameraPosition7;
    public Transform cameraPosition8;

    public bool freeCam1;
    public bool freeCam2;
    public bool freeCam3;
    public bool freeCam4;

    public bool gridCam1;
    public bool gridCam2;
    public bool gridCam3;
    public bool gridCam4;

    private bool leftTrueRightFalse = true;
    public bool LeftTrueRightFalse { get => leftTrueRightFalse; set => leftTrueRightFalse = value; }

    [SerializeField] public float speed = 1.0f;

    public Victory victoryFlag;
    public bool introSequence;

    public Transform cameraPositionStart;

    private bool switchingToFree;
    private bool switchingToGrid;

    public bool level1;
    public bool level2;
    public bool level3;

    void Awake()
    {
        freeCam1 = false;
        freeCam2 = false;
        freeCam3 = false;
        freeCam4 = false;

        gridCam1 = false;
        gridCam2 = false;
        gridCam3 = false;       // Start scene by putting camera on the intro sequence
        gridCam4 = false;       // and make all otehr camera angles false

        introSequence = true;

        switchingToFree = false;
        switchingToGrid = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (introSequence == true)
        {
            speed = 10;
            AtStart();          // Slow speed to start

        }

        else
        {
            SwitchingPlayers();

            transform.LookAt(lookAtTarget.position);

            ChangePosition();

            StayingOnCam();

            SwitchFix();

            SpeedChange();
        }
    }

    void FixedUpdate()
    {
        //transform.LookAt(lookAtTarget.position);

        //StayingOnCam();         // I moved the LookAt and camera movement into FixedUpdate since it calls more frequently
    }                           // I thought it would help make the camera less laggy/jerky

    void AtStart()
    {
        transform.LookAt(victoryFlag.transform.position);

        var step = 5 * speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, cameraPositionStart.position, step);

        if (transform.position == cameraPositionStart.position)
        {
            if (Input.anyKeyDown)
            {
                introSequence = false;
            }
        }
    }

    void ChangePosition()
    {
        if (LeftTrueRightFalse)
        {
            gridCam1 = false;
            gridCam2 = false;       // if left is true (left being free)
            gridCam3 = false;       // then make all grid cameras false
            gridCam4 = false;
            //freeCam1 = true;

            if (Input.GetKeyDown(KeyCode.Alpha1))       // Setting different camera angles must make all others false
            {
                freeCam1 = true;
                freeCam2 = false;
                freeCam3 = false;
                freeCam4 = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                freeCam1 = false;
                freeCam2 = true;
                freeCam3 = false;
                freeCam4 = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                freeCam1 = false;
                freeCam2 = false;
                freeCam3 = true;
                freeCam4 = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                freeCam1 = false;
                freeCam2 = false;
                freeCam3 = false;
                freeCam4 = true;
            }
        }

        else                    // else make all free cameras false                      
        {
            freeCam1 = false;
            freeCam2 = false;
            freeCam3 = false;
            freeCam4 = false;
            //gridCam1 = true;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gridCam1 = true;
                gridCam2 = false;
                gridCam3 = false;
                gridCam4 = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gridCam1 = false;
                gridCam2 = true;
                gridCam3 = false;
                gridCam4 = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                gridCam1 = false;
                gridCam2 = false;
                gridCam3 = true;
                gridCam4 = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                gridCam1 = false;
                gridCam2 = false;
                gridCam3 = false;
                gridCam4 = true;
            }
        }


    }

    void SwitchingPlayers()
    {
        if(LeftTrueRightFalse)
        {
            lookAtTarget = playerLeft.transform;
        }
                                                    // So camera looks at the right player
        else
        {
            lookAtTarget = playerRight.transform;
        }
    }

    void StayingOnCam()             // This makes the camera teleport to stay in position, to prevent stuttering cam from test 1
    {                                          
        //var step = speed * Time.deltaTime;

        // Free
        if (freeCam1)
        {
            transform.position = cameraPosition1.position;
           // transform.position = Vector3.MoveTowards(transform.position, cameraPosition1.position, step);
        }

        if (freeCam2)
        {
            transform.position = cameraPosition2.position;
            //transform.position = Vector3.MoveTowards(transform.position, cameraPosition2.position, step);
            freeCam1 = false;
        }

        if (freeCam3)
        {
            transform.position = cameraPosition3.position;
            //transform.position = Vector3.MoveTowards(transform.position, cameraPosition3.position, step);
            freeCam1 = false;
        }

        if (freeCam4)
        {
            transform.position = cameraPosition4.position;
            //transform.position = Vector3.MoveTowards(transform.position, cameraPosition4.position, step);
            freeCam1 = false;
        }


        // Grid-based
        if (gridCam1)
        {
            transform.position = cameraPosition5.position;
            //transform.position = Vector3.MoveTowards(transform.position, cameraPosition5.position, step);

            if (level1)
            {
                transform.rotation = Quaternion.Euler(47.1f, -26.0f, 0.0f); // Holding rotation on the grid side stops the shaking
            }

            else if (level2)
            {
                transform.rotation = Quaternion.Euler(54.2f, 0.0f, 0.0f);   // Every level has their own rotation
            }                                                               // So I manually set the level bool

            else if (level3)
            {
                transform.rotation = Quaternion.Euler(37.4f, -33.7f, 0.0f);
            }
        }

        if (gridCam2)
        {
            transform.position = cameraPosition6.position;
            //transform.position = Vector3.MoveTowards(transform.position, cameraPosition6.position, step);

            transform.rotation = Quaternion.Euler(85.0f, 0.0f, 0.0f); // For the birds-eye view the rotation should stay at 0,0,0
        }                                                               // for every level

        if (gridCam3)
        {
            transform.position = cameraPosition7.position;
            //transform.position = Vector3.MoveTowards(transform.position, cameraPosition7.position, step);
        }

        if (gridCam4)
        {
            transform.position = cameraPosition8.position;
            //transform.position = Vector3.MoveTowards(transform.position, cameraPosition8.position, step);
        }
    }

    void SwitchFix()            // Fix for switching
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switchingToFree = true;         // Using this bool system allows the camera to move to the other player when switching
            switchingToGrid = false;        // Rather than teleporting
        }                                   // Gives the player a better orientation
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            switchingToGrid = true;
            switchingToFree = false;
        }

        if (switchingToFree)    // When switching between sides, the speed must be lower so the player can see where the players are relative to each other
        {
            speed = 8;
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, cameraPosition1.position, step);

            if (transform.position == cameraPosition1.position)
            {
                freeCam1 = true;
                switchingToFree = false;
            }
        }

        if (switchingToGrid)
        {
            speed = 8;
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, cameraPosition5.position, step);

            if (transform.position == cameraPosition5.position)
            {
                gridCam1 = true;
                switchingToGrid = false;
            }
        }
    }

    

    void SpeedChange()              // Infinite speed is required to prevent camera shake if using the moving mode
    {
        if (transform.position == cameraPosition1.position)
        {
            speed = float.PositiveInfinity;
        }

        else if (transform.position == cameraPosition5.position)
        {
            speed = float.PositiveInfinity;
        }
    }
}