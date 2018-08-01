using UnityEngine;
using System.Collections;

public delegate void DeadEventHandler();

public class PlayerController : Character 
{
    #region Fields

    #region Singleton
    private static PlayerController instance;
    public static PlayerController Instance 
    {
        get 
        {
            if ( instance == null )
            {
                instance = GameObject.FindObjectOfType<PlayerController>();

                DontDestroyOnLoad(instance);

            }
            return instance; 
        }
    }
    #endregion

    public Rigidbody2D MyRigidBody { get; set;}

    private bool allowControl;
    public bool AllowControl 
    { 
        get
        {
            return allowControl;
        }
        set
        {
            this.allowControl = value;

            if (!value)
                MyRigidBody.velocity = Vector2.zero;
        }
    }

    private enum animatorLayer {ShieldOnGround, ShieldOnAir, WithoutShieldOnGround, WithoutShieldOnAir};
    private int currentAnimatorLayer;
    public bool WithoutShield { get; set;}

    public bool Jump { get; set;}
    public float jumpForce;

    public override bool IsDead
    {
        get
        {
            if ( healthStat.CurrentVal <= 0 ) OnDead();

            return healthStat.CurrentVal <= 0;
        }
    }

    #region GroundCheck
    [SerializeField] private Transform[] groundPoints; 
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    #endregion

    #region Combo
    public bool onCombo; 
    private float comboDuration = 1;
    private float comboTimer;
    #endregion

    public event DeadEventHandler Dead;

    private IUseable useable;

    public bool OnWanda { get; set;}

    #endregion

    private SpriteRenderer spriteRenderer;

    #region DONE

    private bool IsGrounded()
    {
        if ( MyRigidBody.velocity.y <= 0 )
        {
            foreach ( Transform point in groundPoints )
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundCheckRadius, whatIsGround);

                for ( int i = 0; i < colliders.Length;  i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void OnDead()
    {
        if ( Dead != null )
        {
            Dead();
        }
    }

    public void SetLayer (int theLayer)
    {
        Anim.SetLayerWeight(currentAnimatorLayer, 0);
        currentAnimatorLayer = theLayer;
        Anim.SetLayerWeight(currentAnimatorLayer, 1);
    }

    private void HandleLayers()
    {
        if ( !OnGround )
        {
            if ( WithoutShield )
                SetLayer((int)animatorLayer.WithoutShieldOnAir);
            else
                SetLayer((int)animatorLayer.ShieldOnAir);
        }  
        else
        {
            if ( WithoutShield )
                SetLayer((int)animatorLayer.WithoutShieldOnGround);
            else
                SetLayer((int)animatorLayer.ShieldOnGround);
        }  
    }

    public void FlipPlayer(float horizontal)
    {
        if( horizontal > 0 && !facingRight || horizontal < 0 && facingRight )
        {
            ChangeDirection();
        }
    }

    private void Use()
    {
        if (useable != null)
            useable.Use();
    }

    public void GiveHealth(float amount)
    {
        healthStat.CurrentVal += amount;
    }

    #endregion DONE

    void Awake()
    {
        instance = this;
    }

    public override void Start () 
    {
        base.Start();

        MyRigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.position = GameManager.Instance.currentCheckpoint.transform.position;

        AllowControl = true;

        facingRight = true;

        SetLayer((int)animatorLayer.ShieldOnGround);
        onCombo = false;
	}
	
    void FixedUpdate()
    {
        if ( !TakingDamage && !IsDead )
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if ( !AllowControl )
            {
                horizontal = 0;
                vertical = 0;
            }

            OnGround = IsGrounded();

            CheckCombo();
            comboTimer -= Time.deltaTime;

            HandleMovement(horizontal, vertical);

            FlipPlayer(horizontal);

            HandleLayers();     
        }
    }

    void Update()
    {
        if ( !TakingDamage && !IsDead )
        {
            if ( AllowControl )
                HandleInput();
        }
    }
     
  

    private void HandleMovement(float horizontal, float vertical)
    {
        if ( !TakingDamage && !IsDead && MyRigidBody.velocity.y < 0 )
           Anim.SetBool("Land", true);

        if ( AllowControl )
        {
            if ( !Attack && OnGround )
                MyRigidBody.velocity = new Vector2(horizontal * movementSpeed, MyRigidBody.velocity.y);

            if ( Jump && MyRigidBody.velocity.y == 0 )
                MyRigidBody.velocity = new Vector2(MyRigidBody.velocity.x , jumpForce);
        }

        Anim.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if ( Input.GetKeyDown(KeyCode.Space) )
            Anim.SetTrigger("Jump");
        
        if (Input.GetKeyDown(KeyCode.A) && !Attack)
        {
            if (!OnGround)
                AttackAnimationToPlay = 0;
            else
                AttackAnimationToPlay = Random.Range(0, (onCombo ? 0 : 3)) + 1;

            Anim.SetInteger("AttackAnimation", AttackAnimationToPlay);
            Anim.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if ( !WithoutShield )
            {
                Anim.SetTrigger("RangedAttack");
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }

    }

    public void ThrowShield(int value)
    {
        if ( !OnGround && value == 1 || OnGround && value == 0 )
        {
            GameObject tmp = (GameObject)Instantiate(projectilePrefab, projectileSpawner.position, projectileSpawner.rotation);

            if ( facingRight )
                tmp.GetComponent<ShieldController>().Initialize(Vector2.right);
            else
                tmp.GetComponent<ShieldController>().Initialize(Vector2.left);    

            WithoutShield = true;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Useable" )
            useable = other.GetComponent<IUseable>();

        base.OnTriggerEnter2D(other);

        if (other.tag == "Bullet")
        {
            StartCoroutine(TakeDamage(other.GetComponent<SoldierBulletController>().damageToGive));
        }

        if (other.tag == "EnemyAttackCollider")
        {
            StartCoroutine(TakeDamage(other.GetComponent<HurtPlayerOnContact>().damageToGive));
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Useable")
            useable = null;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if ( other.collider.tag == "MovingPlatform")
        {
            transform.parent = other.transform;           
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if ( other.collider.tag == "MovingPlatform")
        {
            transform.parent =null;           
        }
    }

    public void FallFromDamage()
    {
        StartCoroutine("FallFromDamageCo");
    }

    public IEnumerator FallFromDamageCo()
    {
        enabled = false;

        Anim.SetTrigger("FallFromDamage");

        yield return new WaitForSeconds(3);
      //  numOfHitsTaked = 0;

        enabled = true;
    }

    private void CheckCombo()
    {
        if ( comboTimer <= 0 )
        {
            onCombo = false;
            comboTimer = 0;
        }
        else
        onCombo = (GameManager.lastEnemyHitted == GameManager.enemyBeingHitted);
    }

    public void StartComboTimer()
    {
        comboTimer = comboDuration;
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
    }
        
    public override IEnumerator TakeDamage(int damage)
    {
        if (!immortal)
        {
            
            healthStat.CurrentVal -= damage;
            AudioSource.PlayClipAtPoint(AttackHitSound, transform.position);

            if (!IsDead)
            {
                Anim.SetTrigger("Damage");
                immortal = true;

             //   StartCoroutine(IndicateImmortal());
                    
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                Anim.SetTrigger("Death");
                AllowControl = false;
            }
        }
    }

    public override void Death()
    {
        if ( IsDead )
        {
            FindObjectOfType<LifeManager>().TakeLife();
//            TimeManager.Instance.ResetTime();
        }

        MyRigidBody.velocity = Vector2.zero;
        Anim.ResetTrigger("Death");

        transform.position = GameManager.Instance.currentCheckpoint.transform.position;

        Anim.SetTrigger("Idle");
        AllowControl = true;
     
        WithoutShield = false;
        healthStat.CurrentVal = healthStat.MaxVal; 
    }

  
}
