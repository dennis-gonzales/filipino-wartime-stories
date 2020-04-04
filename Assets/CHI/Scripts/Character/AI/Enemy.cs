using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private int minAttackDamage = 10;
    [SerializeField] private int maxAttackDamage = 30;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int healthPoint = 100;
    [SerializeField] private EnemyType enemyType = EnemyType._1H;
    [SerializeField] private AttackType attackType = AttackType.Slash;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolSpots;
    [SerializeField] private int currentPatrolIndex = 0;
    [Range(0.5f, 2f)] [SerializeField] private float patrolSpeed = 1f;
    [Range(.1f, 4f)] [SerializeField] private float patrolOverlook = 1f;

    [Header("Chase")]
    [Range(1f, 10f)] [SerializeField] private float awarenessRangeFront = 5f;
    [Range(1f, 10f)] [SerializeField] private float awarenessRangeBack = 2f;
    [Range(2f, 5f)] [SerializeField] private float pursueSpeed = 2f;

    [Header("Collision")]
    [SerializeField] private Transform patrolHitOrigin;
    [SerializeField] private Transform chaseHitOriginFront;
    [SerializeField] private Transform chaseHitOriginBack;

    [Header("Grunt SFX")]
    [SerializeField] private AudioClip[] grunts;
    [SerializeField] private AudioClip[] attackSound;
    [SerializeField] private AudioClip deathSound;

    private const string _playerTag = "Player";
    private const string _patrolSpotTag = "PatrolSpot";
    private const string _hitAnimation = "Hit";

    public event EventHandler OnEnemyDies;

    // Enemy
    private Rigidbody2D enemyAI;
    private Animator animator;
    private HealthSystem healthSystem;
    private EnemyHealth enemyHealth;
    private Vector2 faceFrontDirection;
    private Vector2 faceBackDirection;
    private Vector2 patrolDirection;
    private Transform patrolDestination;
    private RaycastHit2D patrolCastHit;
    private RaycastHit2D chaseCastHitFront;
    private RaycastHit2D chaseCastHitBack;

    // Player
    private GameObject playerObject;
    private Transform playerPosition;
    private CharacterController2D player;
    private AudioSource audio;

    private bool isAlive = true;
    private bool facingRight = true;
    private bool isChasing = false;

    private float attackDelay = 0;

    private enum EnemyType
    {
        _1H,
        _2H
    }

    private enum AttackType
    {
        Slash,
        Jab,
        Charge
        // TODO: add more
        //Shoot
        //Fire,
        //Throw
    }

    private enum State
    {
        Idle,
        Patrol,
        Pursue,
        Dead
    }

    private void Awake()
    {
        Initialization();
        FindPatrolSpot();
    }

    private void Initialization()
    {
        enemyAI = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        enemyHealth = GetComponentInChildren<EnemyHealth>();
        healthSystem = new HealthSystem(healthPoint);

        playerObject = GameObject.FindGameObjectWithTag(_playerTag);
        playerPosition = playerObject.GetComponent<Transform>();
        player = playerObject.GetComponent<CharacterController2D>();

        audio = GetComponent<AudioSource>();
        //counter = timeBetweenAttack;
    }

    private void ChangeHealth(object sender, EventArgs e)
    {
        enemyHealth.SetHealthPercentage(healthSystem.GetHealthPercent());
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged += ChangeHealth;
        }

        if (playerObject != null)
        {
            playerObject.GetComponentInChildren<AnimationEvents>().OnCustomEvent += OnAnimationEvent;
        }
    }

    private void Update()
    {
        if (isAlive)
        {
            faceFrontDirection = facingRight == true ? Vector2.right : Vector2.left;
            faceBackDirection = facingRight == false ? Vector2.right : Vector2.left;

            // TODO: Remove debug on release
            Debug.DrawRay(patrolHitOrigin.position, faceFrontDirection * patrolOverlook);
            patrolCastHit = Physics2D.Raycast(patrolHitOrigin.position, faceFrontDirection, patrolOverlook, whatIsGround);

            Debug.DrawRay(chaseHitOriginFront.position, faceFrontDirection * awarenessRangeFront);
            chaseCastHitFront = Physics2D.Raycast(chaseHitOriginFront.position, faceFrontDirection, awarenessRangeFront, whatIsPlayer);

            Debug.DrawRay(chaseHitOriginBack.position, faceBackDirection * awarenessRangeBack);
            chaseCastHitBack = Physics2D.Raycast(chaseHitOriginBack.position, faceBackDirection, awarenessRangeBack, whatIsPlayer);

            attackDelay += Time.deltaTime;

        }

    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            Process();
            ReInitialize();
        }
    }

    private void Process()
    {

        if (chaseCastHitFront == true)
        {
            if (chaseCastHitFront.collider.CompareTag(_playerTag))
            {
                if (player.GetCurrentHealth() > 0)
                {
                    isChasing = true;
                    Chase();
                }
            }
        }

        else if (chaseCastHitBack == true)
        {
            if (chaseCastHitBack.collider.CompareTag(_playerTag))
            {
                if (player.GetCurrentHealth() > 0)
                {
                    isChasing = true;
                    Chase();
                }
            }
        }

        if (!isChasing)
        {
            if (patrolCastHit == true)
            {
                if (patrolCastHit.collider.CompareTag(_patrolSpotTag))
                {
                    Patrol();
                }
                else
                {
                    // TODO: remove on release
                    
                    Debug.LogWarning(gameObject.name + " is finding his own spot");
                }
            }
        }
    }

    private void ReInitialize()
    {
        isChasing = false;
    }

    private void FindPatrolSpot()
    {
        if (patrolSpots.Length > 0)
        {
            currentPatrolIndex = UnityEngine.Random.Range(0, patrolSpots.Length);
            patrolDestination = patrolSpots[currentPatrolIndex];
        }
        else
        {
            //TODO: remove on release
            Debug.LogWarning("Please add patrol points");
        }
    }

    private void Patrol()
    {

        if (Vector2.Distance(
            patrolHitOrigin.position,
            patrolDestination.position) < patrolOverlook)
        {
            FindPatrolSpot();
        }

        animator.SetBool(AnimateState(State.Patrol), true);
        animator.SetBool(AnimateState(State.Pursue, enemyType), false);

        LeadEnemy(patrolDestination.position, patrolSpeed);
    }

    private void Chase()
    {
        if (patrolCastHit == true) // if the AI is at his patrol spot
        {
            if (patrolCastHit.collider.CompareTag(_patrolSpotTag))
            {

                LeadEnemy(playerPosition.position, pursueSpeed);
                animator.SetBool(AnimateState(State.Patrol), false);;
                animator.SetBool(AnimateState(State.Pursue, enemyType), true);

                // check if the enemy approaching is close to the target
                if (Vector2.Distance(
                    attackPosition.position,
                    playerPosition.position) < attackRange)
                {
                    Attack();
                }
            }
        }

        else
        {
            // back to patrolling
            Patrol();
        }
    }

    private void LeadEnemy(Vector2 direction, float moveSpeed)
    {
        // where is the enemy himself difference to patrol point
        patrolDirection = direction - (Vector2)patrolHitOrigin.position;

        // make sure that the enemy is facing to his patrol point direction
        if (patrolDirection.x < 0f)
        {
            enemyAI.velocity = new Vector2(-moveSpeed, enemyAI.velocity.y);
            if (facingRight)
            {
                Flip();
            }
        }
        else if (patrolDirection.x > 0f)
        {
            enemyAI.velocity = new Vector2(moveSpeed, enemyAI.velocity.y);
            if (!facingRight)
            {
                Flip();
            }
        }

    }

    private void Attack()
    {
        if (player.GetCurrentHealth() > 0 && attackDelay >= 3)
        {
            // ANIMATE ONLY WHEN IS READY (IDLE)
            animator.SetTrigger(AnimateAttack(attackType, enemyType));
            audio.PlayOneShot(attackSound[UnityEngine.Random.Range(0, attackSound.Length)]);
            attackDelay = 0;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDestroy()
    {
        
        if (playerObject != null)
        {
            playerObject.GetComponentInChildren<AnimationEvents>().OnCustomEvent -= OnAnimationEvent;
        }
    }

    private void OnAnimationEvent(string eventName)
    {
        if (eventName == _hitAnimation &&
            Vector2.Distance(
                player.GetAttackPosition,
                transform.position) < 1.5) // if the player's weapon is close enough to damage enemy
        {
            if (grunts != null)
            {
                audio.PlayOneShot(grunts[UnityEngine.Random.Range(0, grunts.Length)]);
                
            }

            TakeDamage(player.GetAttackDamage);
            
        }
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);

        if (healthSystem.GetHealthPoints() <= 0 && isAlive)
        {
            EnemyDead();
        }
    }

    private void EnemyDead()
    {
        isAlive = false;

        OnEnemyDies?.Invoke(this, EventArgs.Empty);

        audio.Stop();

        animator.SetBool(AnimateState(State.Patrol), false);
        animator.SetBool(AnimateState(State.Pursue, enemyType), false);

        audio.PlayOneShot(deathSound);

        animator.Play(UnityEngine.Random.Range(1, 3) == 1 ? "DieFront" : "DieBack", 1);

        enemyAI.velocity = Vector2.zero;
        enemyAI.isKinematic = true;
        enemyAI.GetComponent<CapsuleCollider2D>().enabled = false;

        Destroy(gameObject, 5);
    }

    // ########## ANIMATION ##########
    private bool isAnimating(string anim, int layer)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).IsName(anim);
    }

    private string AnimateState(State enemyState)
    {
        return enemyState.ToString();
    }

    private string AnimateState(State enemyState, EnemyType enemyType)
    {
        return string.Concat(
            enemyState.ToString(),
            enemyType.ToString()
       );
    }

    private string AnimateAttack(AttackType attackType, EnemyType enemyType)
    {
        return string.Concat(
            attackType.ToString(),
            enemyType.ToString()
       );
    }

    // ########## RETURN ##########
    public int GetDamage() => UnityEngine.Random.Range(minAttackDamage, maxAttackDamage);
    public Vector3 GetAttackPosition() => attackPosition.position;

    // ########## DEBUGGING ##########
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Bottom"))
        {
            OnEnemyDies?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }

        if (collision.name.Equals("Bullet(Clone)"))
        {
            if (grunts != null)
            {
                audio.PlayOneShot(grunts[UnityEngine.Random.Range(0, grunts.Length)]);

            }
        }
    }

}
