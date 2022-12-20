using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(ControllerDoor))]
public class ToggleDoor : MonoBehaviour
{
    private ControllerDoor doorController;

    [SerializeField]
    public GameObject doorCollider;

    private int lifted = 0;

    [SerializeField]
    public int target;          // Target number = number of pressure plates needing pressure, regardless of how many are attached

    public int Lifted { get => lifted; set => lifted = value; }

    void Awake()
    {
        doorController = GetComponent<ControllerDoor>();

       // doorCollider = GetComponentInChildren<DoorCollider>();
    }

    void Update()
    {

        if (Lifted == target)
        {
            doorController.OpenDoor();

            doorCollider.SetActive(false);
        }
        else
        {
            doorController.CloseDoor();

            doorCollider.SetActive(true);
        }
    }
}
