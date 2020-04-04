using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AndresBonifacio : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 40f;

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;

    private const string _inputHorizMove = "Horizontal";
    private const string _inputJump = "Jump";
    private const string _inputAttack = "Attack";
    private const string _inputInspect = "Inspect";

    private CharacterController2D controller;
    private AnimationEvents animationEvents;
    private Inspect inspect;

    private float horizontalMove = 0;
    private bool busy = false;
    private bool doJump = false;
    private bool doAttack = false;
    private bool doInspect = false;

    private float attackDelay = 0;

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        inspect = GetComponent<Inspect>();

        animationEvents = GetComponentInChildren<AnimationEvents>();
    }

    private void Start()
    {
        animationEvents.OnCustomEvent += OnAnimationEvent;
    }

    private void OnAnimationEvent(string eventName)
    {
        if (eventName == "Pump")
        {
            Instantiate(bulletPrefab,
                controller.GetAttackPosition,
                transform.rotation);
        }
    }

    private void Update()
    {
        if (controller.IsAlive && !busy)
        {
            Setup();   
        }
    }

    private void Setup()
    {
        horizontalMove = CrossPlatformInputManager.GetAxis(_inputHorizMove) * moveSpeed;

        if (CrossPlatformInputManager.GetButtonDown(_inputJump))
        {
            doJump = true;
        }

        if (CrossPlatformInputManager.GetButtonDown(_inputAttack))
        {
            doAttack = true;
        }

        if (CrossPlatformInputManager.GetButtonDown(_inputInspect))
        {
            doInspect = true;
        }

        attackDelay += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (controller.IsAlive)
        {
            Process();
            Reinitialize();
        }
    }

    private void Process()
    {

        if (doAttack && attackDelay >= 1)
        { 
            controller.RangedAttack();
            attackDelay = 0;
        }

        if (doInspect)
        {
            inspect.Pickup();
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, doJump);
    }

    private void Reinitialize()
    {
        doJump = false;
        doAttack = false;
        doInspect = false;
    }

}
