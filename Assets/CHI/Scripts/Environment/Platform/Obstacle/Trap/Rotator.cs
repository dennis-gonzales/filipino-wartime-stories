using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [Header("Rotation")]
    [SerializeField] private bool Rotating = true;
    [Range(0, 2)] [SerializeField] private int rotation = 2;
    [SerializeField] private float WaitTime = 4f;

    private float counter = 0f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        counter = WaitTime;
    }


    private void Update()
    {
        if (Rotating)
        {
            Rotate();

        }
    }

    private void Rotate()
    {
        if (counter <= 0)
        {
            counter = WaitTime;
            if (rotation == 0)
            {
                Animate("Left");
            }

            else if (rotation == 1)
            {
                Animate("Right");
            }

            else if (rotation == 2)
            {
                Animate("Both");
            }
        }

        else
        {
            counter -= Time.deltaTime;
        }
    }

    private void Animate(string anim)
    {
        animator.SetTrigger(anim);
    }
}
