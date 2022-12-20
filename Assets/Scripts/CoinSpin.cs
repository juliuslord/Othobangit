using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour       // This script is attached to collectables (coins for now) to make them spin //
{
    public Quaternion targetRot;

    [SerializeField]
    public float rotSpeed;

    void Start()
    {
        targetRot.eulerAngles = new Vector3(0, 90, 0);
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed / 100);

        if (transform.rotation.eulerAngles.y >= (targetRot.eulerAngles.y - 45))
        {
            targetRot.eulerAngles = transform.rotation.eulerAngles + new Vector3(0, 90, 0);     // This jibberish works together to a constant spin
        }
    }
}
