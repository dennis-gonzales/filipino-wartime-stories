using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNarration : MonoBehaviour
{
    [Header("Narrative Canvas")]
    [SerializeField] private GameObject narrativeManager;

    [Header("Narrate what?")]
    [SerializeField] private NarrativeType narrativeType = NarrativeType.Original;
    [SerializeField] private Narrative introduction;
    [SerializeField] private Narrative ending;

    private enum NarrativeType
    {
        Original,
        Miserable,
        Independent
    }

    private CharacterController2D playerController;
    private NarrativeSystem narrativeSystem;

    private Enemy[] enemyToKill;
    private int enemyCount = 0;

    private void Awake()
    {

        narrativeSystem = narrativeManager.GetComponent<NarrativeSystem>();

        switch (narrativeType)
        {
            case NarrativeType.Original:
                enemyToKill = FindObjectsOfType<Enemy>();
                InitEnemyToKill();
                break;

            case NarrativeType.Miserable:
                playerController = GameObject.FindGameObjectWithTag("Player")
                    .GetComponent<CharacterController2D>();

                playerController.OnPlayerDies += PlayBadEnd;
                break;

            case NarrativeType.Independent:
                break;
        }
    }


    private void Start()
    {
        if (introduction != null)
        {
            MakeNarrative(introduction);
        }
    }

    private void InitEnemyToKill()
    {
        if (enemyToKill != null)
        {
            enemyCount = enemyToKill.Length;

            foreach (var enemy in enemyToKill)
            {
                enemy.OnEnemyDies += PlayGoodEnd;
            }
        }
    }

    private void PlayGoodEnd(object sender, EventArgs e)
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            if (ending != null)
            {
                PlayEnding();
            }
        }
    }

    private void PlayBadEnd(object sender, EventArgs e)
    {
        if (ending != null)
        {
            PlayEnding();
        }
    }

    public void PlayEnding()
    {
        MakeNarrative(ending);
    }

    public void MakeNarrative(Narrative narrative)
    {
        narrativeSystem.SetNarrative(narrative);
        ShowNarrativeSystem();
    }

    public void ShowNarrativeSystem()
    {
        if (narrativeManager != null)
        {
            narrativeManager.SetActive(true);
        }
    }

    public void HideNarrativeSystem()
    {
        if (narrativeManager != null)
        {
            narrativeManager.SetActive(false);
        }

    }

    public NarrativeSystem GetNarrativeSystem()
    {
        return narrativeSystem;
    }
}
