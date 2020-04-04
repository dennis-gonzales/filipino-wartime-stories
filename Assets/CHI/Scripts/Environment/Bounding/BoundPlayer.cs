using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundPlayer : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField] private Enemy[] enemyToKill;

    [Header("Dialogue")]
    [SerializeField] private Dialogue Bounding_Collided;

    private const string _playerTag = "Player";

    private GameMaster gm;
    private new BoxCollider2D collider;
    private int enemyCount = 0;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        gm = FindObjectOfType<GameMaster>(); 

        InitEnemyToKill();
    }

    private void InitEnemyToKill()
    {
        if (enemyToKill != null)
        {
            enemyCount = enemyToKill.Length;

            foreach (var enemy in enemyToKill)
            {
                enemy.OnEnemyDies += EvaluateKill;
            }
        }
    }

    private void EvaluateKill(object sender, EventArgs e)
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_playerTag))
        {
            if (Bounding_Collided != null)
            {
                gm.MakeDialog(Bounding_Collided);
            }
        }
    }

}
