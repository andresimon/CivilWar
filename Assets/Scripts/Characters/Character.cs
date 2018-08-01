using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour 
{
    #region Fields

    public Animator Anim { get; private set;}

    public float movementSpeed;

    protected bool facingRight;

    public Transform projectileSpawner;
    public GameObject projectilePrefab;

    public bool Attack { get; set;}
    public int AttackAnimationToPlay { get; set;}

    public bool OnGround { get; set;}

    public bool TakingDamage { get; set;}

    [SerializeField] protected Stat healthStat;

    public abstract bool IsDead { get; }

    public AudioClip meleeAttackSound;
    public AudioClip AttackHitSound;

    public int NumOfHitsTaked { get; set;}
    public int hitsBeforeFall;

    protected bool immortal;
    [SerializeField] protected float immortalTime;

    #endregion

	public virtual void Start ()
    {
        Anim = gameObject.GetComponent<Animator>();

        healthStat.Initialize();
	}
	
    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;

        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void MeleeAttack() 
    {
        AudioSource.PlayClipAtPoint(meleeAttackSound, transform.position);
    }

    public abstract void Death();

    public abstract IEnumerator TakeDamage(int damage);

    public virtual void OnTriggerEnter2D (Collider2D other)
    {

    }

}
