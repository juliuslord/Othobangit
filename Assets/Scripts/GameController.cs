using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour // This is attached to the Game Controller object //
{                                           // it handles which side the player wants to be on
    public LookAt mainCamera;
    public PlayerController leftPlayer;
    public GridPlayer rightPlayer;

    void Start()
    {
        
    }

    void Update()
    {
        SetSide();
    }

    void SetSide()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))           // Controlling which side of the game the camera looks at
        {
            mainCamera.LeftTrueRightFalse = true;
            leftPlayer.LeftTrueRightFalse = true;
            rightPlayer.LeftTrueRightFalse = true;
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            mainCamera.LeftTrueRightFalse = false;
            leftPlayer.LeftTrueRightFalse = false;
            rightPlayer.LeftTrueRightFalse = false;
        }
    }
}
