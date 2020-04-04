using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string _playerTag = "Player";
    [Header("Bullet")]
    [SerializeField] private float travelSpeed = 20f;
    [SerializeField] private int bulletMissedDestroyTime = 2;

    [SerializeField] private int attackDamage;

    private Enemy enemy;
    private Rigidbody2D bulletRb;

    private CharacterController2D playerController;

    private void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        bulletRb.velocity = transform.right * travelSpeed;

        playerController = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<CharacterController2D>();

        attackDamage = playerController.GetAttackDamage;
        
        StartCoroutine(DestroyBulletInTime());
    }

    private IEnumerator DestroyBulletInTime()
    {
        yield return new WaitForSeconds(bulletMissedDestroyTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            Destroy(gameObject);
            enemy.TakeDamage(attackDamage);
        }
        
    }
}
