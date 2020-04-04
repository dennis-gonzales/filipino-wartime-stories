using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterController2D : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private bool onGround;

    [Header("Player")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private TextMeshProUGUI damageIndicator;
    [SerializeField] private int minAttackDamage = 15;
    [SerializeField] private int maxAttackDamage = 30;
    [SerializeField] private int healthPoints = 100;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float respawnTime = 2;
    [SerializeField] private int damageIncreaseExpireTime = 20;
    [SerializeField] private CharacterType charType = CharacterType.Melee;

    [Header("Health System")]
    [SerializeField] private Image healthBarImage;
    [SerializeField] private TextMeshProUGUI healthPointText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Walk")]
    [SerializeField] private AudioClip walkSfx;

    [Header("Meele")]
    [SerializeField] private AudioClip[] playerSwordSwing;
    [SerializeField] private AudioClip swordSwing;

    [Header("Ranged")]
    [SerializeField] private AudioClip[] gunShots;

    [Header("Grunt")]
    [SerializeField] private AudioClip[] grunts;
    [SerializeField] private AudioClip[] died;

    [Header("Jump")]
    [SerializeField] private AudioClip[] airJump;
    [SerializeField] private AudioClip landingJump;

    
    private enum CharacterType
    {
        Melee,
        Range
    }

    private const string _hitAnimation = "Hit";

    public event EventHandler OnPlayerDies;

    private GameMaster gm;
    private Rigidbody2D playerRigidbody;
    private HealthSystem healthSystem;
    private Collider2D[] groundCollider;
    private new CapsuleCollider2D collider;
    private Animator animator;
    private List<Enemy> enemyList = new List<Enemy>();
    private AudioSource audio;

    private Vector3 targetVelocity;

    private bool isAlive = true;
    private bool facingRight = true;
    private float groundRadius = 0.2f;
    private float newPos = 0f;
    private int damageTaken = 0;

    private void Awake()
    {
        gm = FindObjectOfType<GameMaster>();

        playerRigidbody = GetComponent<Rigidbody2D>();

        animator = GetComponentInChildren<Animator>();

        audio = GetComponent<AudioSource>();

        collider = GetComponent<CapsuleCollider2D>();

        healthSystem = new HealthSystem(maxLives, healthPoints);
    }

    private void Start()
    {
        InitLives();
        InitHealth();
        InitDamageIndicator();
        WhatIsEnemy();
    }

    private void InitLives()
    {
        for (int i = 0; i < hearts.Length; i++)
        {

            if (i < healthSystem.GetLivesCount())
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxLives)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void InitHealth()
    {
        healthSystem.OnHealthChanged += ChangeHealth;
        healthPointText.text = string.Concat(GetCurrentHealth(), "%");
    }

    private void InitDamageIndicator()
    {
        damageIndicator.text = string.Concat(
            minAttackDamage.ToString() + " - " + maxAttackDamage.ToString()
            );
    }

    private void ChangeHealth(object sender, EventArgs e)
    {
        healthBarImage.fillAmount = healthSystem.GetHealthPercent();
        healthPointText.text = string.Concat(GetCurrentHealth(), "%");
    }

    private void WhatIsEnemy()
    {
        enemyList.AddRange(FindObjectsOfType<Enemy>());
        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                // TODO: make a cache for this object
                enemy.GetComponentInChildren<AnimationEvents>().OnCustomEvent += OnAnimationEvent;
            }
        }
    }

    private void OnAnimationEvent(string eventName)
    {
        if (eventName == _hitAnimation)
        {
            foreach (Enemy enemy in enemyList)
            {
                if (enemy != null)
                {
                    if (Vector2.Distance(
                            enemy.GetAttackPosition(),
                            transform.position) < 1.5)
                    {

                        if (grunts != null)
                        {
                            audio.PlayOneShot(grunts[UnityEngine.Random.Range(0, grunts.Length)]);       
                        }

                        animator.SetTrigger(_hitAnimation);

                        if (GetCurrentHealth() > 0 && isAlive)
                        {
                            damageTaken = GetCurrentHealth() - enemy.GetDamage();
                            StartCoroutine(DepleteHealth(damageTaken <= 0 ? 0 : damageTaken));
                        }

                    }

                }
            }

        }

    }

    public void Respawn()
    { // method called outside class
        isAlive = false;
        StartCoroutine(DepleteHealth(0));
    }

    private IEnumerator DepleteHealth(int targetHealth)
    {
        if (GetCurrentHealth() <= 0)
        {
            isAlive = false;
        }

        yield return new WaitForSeconds(Time.deltaTime);

        if (GetCurrentHealth() <= 0)
        {
            Dead();
        }

        else if (targetHealth < GetCurrentHealth())
        {
            healthSystem.TakeDamage(1);
            StartCoroutine(DepleteHealth(targetHealth));
        }
    }

    private void Dead()
    {
        audio.PlayOneShot(died[UnityEngine.Random.Range(0, died.Length)]);
        isAlive = false;
        PlayerDead();
        StartCoroutine(RespawnPlayer(gm.lastCheckPointPos));
    }

    private IEnumerator RespawnPlayer(Vector3 spawnPos)
    {

        healthSystem.TakeLives(1);
        InitLives();

        yield return new WaitForSeconds(respawnTime);

        // check if there's live available

        if (healthSystem.GetLivesCount() > 0)
        {
            healthSystem.Heal(healthPoints);
            RespawnPlayer();
            transform.position = spawnPos;
            isAlive = true;
        }

        else
        {
            OnPlayerDies?.Invoke(this, EventArgs.Empty);
        }
    }

    private void FixedUpdate()
    {
        onGround = false;

        groundCollider = Physics2D.OverlapCircleAll(
            groundPosition.position,
            groundRadius,
            whatIsGround
        );

        GroundCheck();
    }


    private void GroundCheck()
    {
        for (int i = 0; i < groundCollider.Length; i++)
        {
            if (groundCollider[i].gameObject != gameObject)
            {
                transform.rotation = Quaternion.Euler(
                    transform.rotation.x,
                    facingRight == true ? 0 : 180,
                    groundCollider[i].gameObject.transform.rotation.z
                );

                onGround = true;
                
            }
        }

        if (OnGround)
        {
            if (!audio.isPlaying && playerRigidbody.velocity.y < 0f)
            {
                audio.PlayOneShot(landingJump);
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (onGround || m_AirControl)
        {

            newPos = move * 10f;

            if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Jump") && onGround)
            {
                animator.SetFloat("Move", Mathf.Abs(newPos));

                if (!audio.isPlaying && move != 0)
                {
                    audio.PlayOneShot(walkSfx);
                }
            }

            // Move the character by finding the target velocity
            targetVelocity = new Vector2(newPos, playerRigidbody.velocity.y);
            // And then smoothing it out and applying it to the character
            playerRigidbody.velocity = targetVelocity;

            // If the input is moving the player right and the player is facing left...
            if (newPos > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // if the input is moving left and the player is facing right
            else if (newPos < 0 && facingRight)
            {
                Flip();
            }
        }



        // If the player should jump...
        if (onGround && jump)
        {
            // Add a vertical force to the player.

            if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Jump"))
            {
                onGround = false;
                animator.SetTrigger("Jump");
                playerRigidbody.AddForce(Vector2.up * jumpForce);

                audio.PlayOneShot(airJump[UnityEngine.Random.Range(0, airJump.Length)]);

            }

        }

    }

    public void MeleeAttack()
    {
        if (charType == CharacterType.Melee)
        {
            audio.PlayOneShot(playerSwordSwing[UnityEngine.Random.Range(0, playerSwordSwing.Length)]);
            audio.PlayOneShot(swordSwing);
            animator.SetTrigger(Time.frameCount % 2 == 0 ? "Slash" : "Jab");
        }
    }

    public void RangedAttack()
    {
        if (charType == CharacterType.Range)
        {
            audio.PlayOneShot(gunShots[UnityEngine.Random.Range(0, gunShots.Length)]);
            animator.SetTrigger("Fire");
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void PlayerDead()
    {
        print("Dead");
        animator.SetFloat("Move", 0);
        animator.SetInteger("Dead", UnityEngine.Random.Range(1, 3));
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.isKinematic = true;
        collider.enabled = false;

    }

    public void RespawnPlayer()
    {
        print("Respawned");
        playerRigidbody.isKinematic = false;
        collider.enabled = true;
        animator.SetInteger("Dead", 0);

    }

    public void Trap(int damage)
    {
        if (GetCurrentHealth() > 0 && isAlive)
        {
            damageTaken = GetCurrentHealth() - damage;
            StartCoroutine(DepleteHealth(damageTaken <= 0 ? 0 : damageTaken));
        }
    }

    public void Land(Transform objectToLand)
    {
        if (onGround)
        {
            transform.SetParent(objectToLand);
        }
        else
        {
            StepOut();
        }
    }

    public void StepOut()
    {
        transform.SetParent(null);
    }

    private void OnDestroy()
    {
        healthSystem.OnHealthChanged -= ChangeHealth;

        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.GetComponentInChildren<AnimationEvents>().OnCustomEvent -= OnAnimationEvent;
            }
        }
    }

    public void IncreaseDamage(int damageIncrease)
    {
        StartCoroutine(DamageLimiter(damageIncrease));
    }

    private IEnumerator DamageLimiter(int damageIncrease)
    {
        minAttackDamage += damageIncrease;
        maxAttackDamage += damageIncrease;
        InitDamageIndicator();

        yield return new WaitForSeconds(damageIncreaseExpireTime);

        minAttackDamage -= damageIncrease;
        maxAttackDamage -= damageIncrease;
        InitDamageIndicator();
    }

    public void IncreaseLives(int maxLives)
    {
        this.maxLives += maxLives;
        healthSystem.IncreaseLives(maxLives);
        InitLives();
    }

    public void Regenerate(int amountToRegen)
    {

        int newHealth = amountToRegen + GetCurrentHealth();

        if (newHealth > healthSystem.GetMaxHealth)
        {
            newHealth = healthSystem.GetMaxHealth;
        }

        StartCoroutine(RegenHealth(newHealth));
    }

    private IEnumerator RegenHealth(int healthToRegen)
    {

        yield return new WaitForSeconds(Time.deltaTime);

        if (GetCurrentHealth() < healthToRegen)
        {
            healthSystem.Heal(1);
            StartCoroutine(RegenHealth(healthToRegen));
        }

    }

    public Vector3 GetAttackPosition => attackPosition.position;

    public bool OnGround => onGround;

    public bool IsAlive => isAlive;

    public int GetAttackDamage => UnityEngine.Random.Range(minAttackDamage, maxAttackDamage);

    public int GetCurrentHealth() { return healthSystem.GetHealthPoints(); }

}
