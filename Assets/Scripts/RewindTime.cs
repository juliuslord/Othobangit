using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTime : MonoBehaviour // This script is attached to all moving objects //
{
    public bool isRewinding = false;        // This is entirely from https://www.youtube.com/watch?v=eqlHpPzS22U
                                            // It makes fixing mistakes easier than resetting the whole level
    [SerializeField]                        // 
    public float recordTime = 1f;           // Seconds rewound

    List<PointInTime> pointsInTime;

    // Start is called before the first frame update
    void Start()
    { 
        pointsInTime = new List<PointInTime>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRewind();
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            StopRewind();
        }
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0]; 

            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }

        else
        {
            StopRewind();
        }
    }

    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }

    public struct PointInTime
    {

        public Vector3 position;
        public Quaternion rotation;

        public PointInTime(Vector3 _position, Quaternion _rotation)
        {
            position = _position;
            rotation = _rotation;
        }
    }
}