using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChoice : MonoBehaviour
{

    [Header("Choice Canvas")]
    [SerializeField] private GameObject choiceManager;

    [Header("Choice what?")]
    [SerializeField] private ChoiceType choiceType = ChoiceType.Kill;

    private enum ChoiceType
    {
        Kill,
        Reach
    }

    private ChoiceSystem choiceSystem;
    private Enemy[] enemyToKill;
    private int enemyCount = 0;

    private void Awake()
    {
        choiceSystem = choiceManager.GetComponent<ChoiceSystem>();

        switch (choiceType)
        {
            case ChoiceType.Kill:
                enemyToKill = FindObjectsOfType<Enemy>();
                InitEnemyToKill();
                break;

            case ChoiceType.Reach:
                break;
        }

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
            choiceManager.SetActive(true);
        }
    }

    public void ChoiceMaker()
    {
        choiceManager.SetActive(true);
    }
}
