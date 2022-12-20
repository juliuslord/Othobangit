using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMagLight : MonoBehaviour
{
    public bool Attract;
    public bool Repel;

    public Light lightSource;

    void Awake()
    {
        lightSource = GetComponentInChildren<Light>();

        Attract = false;
        Repel = false;
    }
   
    void Update()
    {
        InputtingMagnetism();
        MagentismLight();
    }

    void InputtingMagnetism()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Attract)
            {
                Attract = false;
                Repel = false;
            }

            else
            {
                Attract = true;
                Repel = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Repel)
            {
                Attract = false;
                Repel = false;
            }

            else
            {
                Attract = false;
                Repel = true;
            }
        }
    }

    void MagentismLight()
    {
        if (Attract == true)
        {
            lightSource.color = Color.green;
        }

        else if (Repel == true)
        {
            lightSource.color = Color.blue;
        }

        else
        {
            lightSource.color = Color.clear;
        }
    }
}
