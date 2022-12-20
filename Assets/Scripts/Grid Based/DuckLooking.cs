using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckLooking : MonoBehaviour    // This is just so the ducks look at the player in the 1st level
{
    // a r t
        
    public GridPlayer gridPlayer;

    void Update()
    {
        transform.LookAt(gridPlayer.transform.position);
    }
}
