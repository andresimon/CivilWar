using UnityEngine;
using System.Collections;

public class SoldierBulletController : MonoBehaviour 
 {
    #region Fields

    public float moveSpeed;

    private Rigidbody2D rd;

    public int damageToGive;

    private Vector2 direction;

    #endregion

    void Start () 
    {
        rd = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update () 
    {
        rd.velocity = direction * moveSpeed;
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        Destroy(gameObject);
    }

}
