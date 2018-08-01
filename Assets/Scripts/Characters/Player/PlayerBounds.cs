using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour
{
    private float xMin;
 
    private float xMax;

    void LateUpdate ()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax),
                                        transform.position.y, transform.position.z);
    }

    public void SetBounds(float xMin, float xMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
    }
}
