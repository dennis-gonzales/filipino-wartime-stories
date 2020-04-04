using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private const string _playerTag = "Player";
    [Header("Chest")]
    [SerializeField] private Treasure reward = Treasure.Empty;
    [SerializeField] private bool claimed = false;
    [SerializeField] private bool opened = false;

    [Header("Sprite Renderer")]
    [SerializeField] private new SpriteRenderer renderer;

    [Header("Sprite")]
    [SerializeField] private Sprite attack;
    [SerializeField] private Sprite heart;
    [SerializeField] private Sprite potion;
    [SerializeField] private Sprite keys;

    [Header("Chest Dialogue")]
    [SerializeField] private Dialogue unlocked;
    [SerializeField] private Dialogue empty;

    [Header("Reward Dialogue")]
    [SerializeField] private Dialogue damageIncrease;
    [SerializeField] private Dialogue regeneration;
    [SerializeField] private Dialogue extraLives;
    [SerializeField] private Dialogue key;

    [Header("Reward Properties")]
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private int regenAmount = 50;
    [SerializeField] private int liveAmount = 1;

    [Header("Clip")]
    [SerializeField] private AudioClip chest;
    [SerializeField] private AudioClip clipDmg;
    [SerializeField] private AudioClip clipRegen;
    [SerializeField] private AudioClip clipLives;
    [SerializeField] private AudioClip clipKey;
    [SerializeField] private AudioClip clipEmpty;

    private GameMaster gm;
    private Animator animator;
    private AudioSource audio;

    private GameObject playerObject;
    private CharacterController2D playerController;
    private Inspect playerInspect;

    private enum Treasure
    {
        Damage_Increase,
        Extra_Lives,
        Regeneration,
        Key,
        Empty

    }

    private void Awake()
    {
        gm = FindObjectOfType<GameMaster>();
        animator = GetComponent<Animator>();

        playerObject = GameObject.FindGameObjectWithTag(_playerTag);
        playerController = playerObject.GetComponent<CharacterController2D>();
        playerInspect = playerObject.GetComponent<Inspect>();

        audio = GetComponent<AudioSource>();
    }

    public void Unlock()
    {
        animator.SetTrigger("Open");

        if (opened)
        {
            if (claimed)
            {
                if (reward == Treasure.Empty)
                {
                    gm.MakeDialog(empty);
                }

                else
                {
                    gm.MakeDialog(unlocked);
                }
                
            }

            else
            {
                Claim();
            }
        }

        else
        {
            Evaluate();
        }

        opened = true;
    }

    private void Evaluate()
    {

        audio.PlayOneShot(chest);

        switch (reward)
        {
            case Treasure.Damage_Increase:
                renderer.sprite = attack;
                break;

            case Treasure.Extra_Lives:
                renderer.sprite = heart;
                break;

            case Treasure.Regeneration:
                renderer.sprite = potion;
                break;

            case Treasure.Key:
                renderer.sprite = keys;
                break;

            case Treasure.Empty:
                break;

        }
    }

    private void Claim()
    {
        claimed = true;
        renderer.sprite = null;

        switch (reward)
        {
            case Treasure.Damage_Increase:
                playerController.IncreaseDamage(damageAmount);
                audio.PlayOneShot(clipDmg);
                gm.MakeDialog(damageIncrease);
                break;

            case Treasure.Regeneration:
                playerController.Regenerate(regenAmount);
                audio.PlayOneShot(clipRegen);
                gm.MakeDialog(regeneration);
                break;

            case Treasure.Extra_Lives:
                playerController.IncreaseLives(liveAmount);
                audio.PlayOneShot(clipLives);
                gm.MakeDialog(extraLives);
                break;

            case Treasure.Key:
                playerInspect.SetKey(true);
                audio.PlayOneShot(clipKey);
                gm.MakeDialog(key);
                break;

            case Treasure.Empty:
                audio.PlayOneShot(clipEmpty);
                gm.MakeDialog(empty);
                break;
        }
    }
}
