using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour    // This script is attached to the target on the grid in each level //
{
    [SerializeField] private MenuController _winMenu;

    void Start()
    {
        _winMenu.Won = false;   // At the start, you didn't win
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Only if player enters
        {
            Debug.Log("Level complete, moving on...");

            _winMenu.Won = true;    // Winning sent to the menu to change
        }
    }
}
