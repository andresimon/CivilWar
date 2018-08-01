using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;
    public float moveSpeed;

    public Transform nextPoint;
    public Transform[] points;

    public int pointSelection;

	void Start () 
    {
        nextPoint = points[pointSelection];
	}
	
	void Update () 
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, nextPoint.position, Time.deltaTime * moveSpeed);

        if (platform.transform.position == nextPoint.position)
        {
            // put a timer
            pointSelection++;

            if (pointSelection == points.Length)
                pointSelection = 0;

            nextPoint = points[pointSelection];
        }
	}

}
