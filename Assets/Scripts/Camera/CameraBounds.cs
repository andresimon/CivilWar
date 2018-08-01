using UnityEngine;
using System.Collections;

public class CameraBounds : MonoBehaviour 
{
    [SerializeField]private float xMin;
    public float XMin { get{ return xMin; }}

    [SerializeField]private float xMax;

    [SerializeField]private float yMin;
    [SerializeField]private float yMax;

    private Transform target;
    public Transform Target { get{ return target;} set { target = value;}}

	void Start () 
    {
        target = GameObject.Find("Player").transform;
	}
	
	void LateUpdate ()
    {
        if ( target)
            transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax),
                                             Mathf.Clamp(target.position.y, yMin, yMax),
                                             transform.position.z);
	}

    public void SetBounds(float xMin, float xMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
    }

}
