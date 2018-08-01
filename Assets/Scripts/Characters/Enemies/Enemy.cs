using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Enemy : Character
{
    #region Fields

	private IEnemyState currentState;

    public GameObject Target{ get; set;}

    public AudioClip deadSound;

    public float meleeRange;
    public float rangedRange;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;

            return false;
        }
    }

    public bool InRangedRange
    {
        get
        {
            if (Target != null)
                return Vector2.Distance(transform.position, Target.transform.position) <= rangedRange;

            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return healthStat.CurrentVal <= 0;
        }
    }
   
    private bool canMove = true;
    private bool moveOnPatrol;

    #region EdgeCheck
    [SerializeField] private Transform edgeCheck;
    public float edgeCheckRadius;
    public LayerMask whatIsWall;
    public bool AtEdge { get; set; }
    #endregion

//    #region WallCheck
//    public Transform wallCheck;
//    public float wallCheckRadius;
//    private bool hittingWall;
//    #endregion

    private Canvas healthCanvas;

    public bool isBoss;

    private bool isKnockout;
    public bool IsKnockout { get { return isKnockout; } set { isKnockout = value; } }

    private SpriteRenderer spriteRenderer;

    #endregion

    void Awake()
    {
        facingRight = !isBoss;
    }

    public void Initialize(bool facingRight)
    {
        if (!facingRight)
            ChangeDirection();
    }

    void Update()
    {
        if ( !IsDead && !IsKnockout )
        {
            if ( !TakingDamage )
            {
                currentState.Execute();
            }

            LookAtTarget();        
        }
    }

    private void LookAtTarget()
    {
        if ( Target != null )
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
                ChangeDirection();
        }
    }

    public void RemoveTarget()
    {
        Target = null;

        ChangeState(new PatrolState());
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void ChangeDirection()
    {
        Transform tmp = transform.Find("EnemyHealthBarCanvas").transform;

        Vector3 pos = tmp.position;

        tmp.SetParent(null);

        base.ChangeDirection();

        tmp.SetParent(transform);

        tmp.position = pos;
    }

    public override void Start()
    {
        base.Start();
       
        if ( PlayerController.Instance )
            PlayerController.Instance.Dead += new DeadEventHandler(RemoveTarget);

        moveOnPatrol = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeState(new PatrolState());

        healthCanvas = transform.GetComponentInChildren<Canvas>();
    }

    void FixedUpdate()
    {
        AtEdge = !Physics2D.OverlapCircle(edgeCheck.position, edgeCheckRadius, whatIsWall);
//        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
    }

    public void ChangeState(IEnemyState newState)
    {
        if (SceneManager.GetActiveScene().name == "Level01_Intro")
        {
            newState = new PatrolState();
        }
        
        if (currentState != null) currentState.Exit();

        currentState = newState;

//        Debug.LogWarning(currentState.ToString());

        currentState.Enter(this);
    }

    public void Move()
    {
        if (!OnGround) return;
        
        if (!Attack && moveOnPatrol)
        {
            if ( !AtEdge  )
            {
                Anim.SetFloat("Speed", 1);
                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolState) 
            {
                Target = null;
                ChangeDirection();
                ChangeState(new IdleState());
            }
            else if (currentState is RangedState) 
            {
                Target = null;
                ChangeState(new IdleState());
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if ( other.tag == "Shield" )
        {
            StartCoroutine(TakeDamage(other.GetComponent<ShieldController>().damageToGive));
        }

        if ( other.tag == "PlayerAttackCollider" )
        {
            StartCoroutine(TakeDamage(other.GetComponent<HurtEnemyOnContact>().damageToGive));
        }

        currentState.OnTriggerEnter(other);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if ( other.collider.tag == "Ground" || other.collider.tag == "Platform")
        {
            OnGround = true;
        }
    }

    public void ThrowProjectile(int value)
    {
        if ( facingRight )
        {
            GameObject tmp = (GameObject)Instantiate(projectilePrefab, projectileSpawner.position, projectileSpawner.rotation); 
            tmp.GetComponent<SoldierBulletController>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(projectilePrefab, projectileSpawner.position, Quaternion.Inverse(projectileSpawner.rotation)); 
            tmp.GetComponent<SoldierBulletController>().Initialize(Vector2.left);    
        }
    }

    public override IEnumerator TakeDamage(int damage)
    {
        if ( !immortal )
        {
            if (!healthCanvas.isActiveAndEnabled)
                healthCanvas.enabled = true;

            healthStat.CurrentVal -= damage;

            if (!IsDead && !IsKnockout)
            {
                AudioSource.PlayClipAtPoint(AttackHitSound, transform.position);

                if ( NumOfHitsTaked > hitsBeforeFall && isBoss )
                {
                    Anim.SetTrigger("Knockout");
                    NumOfHitsTaked = 0;
                }
                else
                    Anim.SetTrigger("Damage");
            }
            else
            {
                float healthDropProbability = UnityEngine.Random.Range(0, 100);
                if ( GameManager.Instance.HealhDropProbability >= healthDropProbability )
                {
                    Instantiate(GameManager.Instance.HealthPrefab, 
                        new Vector3(transform.position.x, transform.position.y + 5), Quaternion.identity);
                }

                AudioSource.PlayClipAtPoint(deadSound, transform.position);
                Anim.SetTrigger("Death");

                //ScoreManager.AddPoints(pointsOnDeath);

                healthCanvas.enabled = false;

                yield return null;
            }
        }
    }

    private IEnumerator IndicateImmortal()
    {
        while ( immortal )
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(immortalTime);

    }

    public override void Death()
    {
        Destroy(gameObject); 
    }

    void OnBecameVisible()
    {
        moveOnPatrol = canMove;
    }

    private IEnumerator KnockoutCo()
    {
        immortal = true;

        StartCoroutine(IndicateImmortal());

        yield return new WaitForSeconds(immortalTime);

        immortal = false;
        isKnockout = false;
    }

    public void Knockout()
    {
        ChangeState(new PatrolState());
        StartCoroutine(KnockoutCo());
    }
}
