using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class ShieldController : MonoBehaviour 
{
    #region Fields

    public float moveSpeed;

    private Rigidbody2D rd;

    public int damageToGive;

    private bool backing;

    private Vector2 direction;

    public float returnCooldown;
    private float returnCooldownTimer;

    #endregion

    void Start () 
    {
        rd = gameObject.GetComponent<Rigidbody2D>();
	}
	
    void Update () 
    {
        ReturnAfterTime();

        if ( !backing )
            rd.velocity = direction * moveSpeed;
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * moveSpeed);
        }
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        if ( backing )
        {
            if ( other.tag == "Player") 
            {
                PlayerController.Instance.WithoutShield = false;
                Destroy(gameObject);
            }         
        }
        ReturnShield();
    }

    private void ReturnShield()
    {
        rd.velocity = Vector2.zero;
        backing = true;  
    }

    void OnBecameInvisible()
    {
        //ReturnShield();
    }

    void ReturnAfterTime()
    {
        if ( PlayerController.Instance.IsDead )
            Destroy(gameObject);

        if ( !backing )
        {
            if (returnCooldownTimer >= returnCooldown)
            {
                ReturnShield();
                returnCooldownTimer = 0;
            }
            else
                returnCooldownTimer += Time.deltaTime;
        }
    }
}
