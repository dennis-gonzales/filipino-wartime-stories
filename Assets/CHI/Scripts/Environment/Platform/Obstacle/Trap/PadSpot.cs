using System;
using UnityEngine;

public class PadSpot : MonoBehaviour
{
    private const string _playerTag = "Player";
    [Header("Pad")]
    [SerializeField] private Transform[] spotPoints;
    [SerializeField] private bool turnXorY = false;
    [Range(.5f, 3f)] [SerializeField] private float moveSpeed = 1f;

    private Transform currentSpotPoint;
    private CharacterController2D player;
    private bool turn = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<CharacterController2D>();
    }

    private void FixedUpdate()
    {
        if (turnXorY)
        {
            TurnX();
        }
        else
        {
            turnY();
        }
    }

    private void TurnX()
    {
        if (transform.position.x > spotPoints[0].position.x)
        {
            turn = false;
        }

        if (transform.position.x < spotPoints[1].position.x)
        {
            turn = true;
        }

        if (turn)
        {
            transform.position = new Vector2(
                transform.position.x + moveSpeed * Time.deltaTime,
                transform.position.y);
        }

        else
        {
            transform.position = new Vector2(
                transform.position.x - moveSpeed * Time.deltaTime,
                transform.position.y);
        }
    }

    private void turnY()
    {
        if (transform.position.y > spotPoints[0].position.y)
        {
            turn = false;
        }

        if (transform.position.y < spotPoints[1].position.y)
        {
            turn = true;
        }

        if (turn)
        {
            transform.position = new Vector2(
                transform.position.x,
                transform.position.y + moveSpeed * Time.deltaTime);
        }

        else
        {
            transform.position = new Vector2(
                transform.position.x,
                transform.position.y - moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_playerTag))
        {
            player.Land(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_playerTag))
        {
            player.StepOut();
        }
    }
}
