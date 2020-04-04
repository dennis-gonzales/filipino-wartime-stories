using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour
{
    private const string _playerTag = "Player";

    private CharacterController2D player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            player.Respawn();
        }
    }
    
}
