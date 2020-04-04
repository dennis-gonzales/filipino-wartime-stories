using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private const string _playerTag = "Player";
    [Header("Saw")]
    [SerializeField] private float sawSpeed = 3f;
    [SerializeField] private Transform[] sawPoints;

    private CharacterController2D playerController;

    private bool turn = false;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<CharacterController2D>();
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, sawPoints[0].position) < 0.1)
        {
            turn = false;
            
        }

        else if (Vector2.Distance(transform.position, sawPoints[1].position) < 0.1)
        {
            turn = true;
        }

        if (turn)
        {
            transform.position = Vector2.MoveTowards(transform.position, sawPoints[0].position, sawSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, sawPoints[1].position, sawSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerController.Land(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerController.StepOut();
    }
}
