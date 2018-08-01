using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Falcon : MonoBehaviour 
{
    #region Fields

    public float moveSpeed;

    private Rigidbody2D rd;

    private Vector2 direction;

    #endregion

    void Start () 
    {
        rd = gameObject.GetComponent<Rigidbody2D>();
        direction = Vector2.right;
    }

    void Update () 
    {
        rd.velocity = direction * moveSpeed;
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

}
