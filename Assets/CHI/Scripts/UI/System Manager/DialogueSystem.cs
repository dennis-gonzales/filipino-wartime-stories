using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    //[SerializeField] private Animator animator;

    private GameMaster gm;
    private Dialogue dialogue;
    private int sentencesCount = 0;
    private int index = 0;

    private void Awake()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnEnable()
    {
        Setup();

    }

    private void Setup()
    {
        if (dialogue != null)
        {
            sentencesCount = dialogue.sentence.Length;
            NextSentence();
        }
    }

    //private void Start()
    //{
    //    Setup();
    //}

    public void NextSentence()
    {
        //animator.SetTrigger("Fade");
        if (dialogue != null && index < sentencesCount)
        {
            dialogueText.text = string.Empty;
            StartCoroutine(Type());
            index++;
        }

        else
        {
            gm.HideDialogueSystem();
        }
    }

    private IEnumerator Type()
    {
        foreach (char letter in dialogue.sentence[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        index = 0;
        dialogueText.text = string.Empty;
    }

    public void SetDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue;
    }
}
