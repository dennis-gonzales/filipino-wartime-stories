using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarrativeSystem : MonoBehaviour
{

    public event EventHandler OnNarrationHide;

    [Header("Narration")]
    [SerializeField] private TextMeshProUGUI narrationText;
    [Range(0f, 0.5f)]
    [SerializeField] private float typingSpeed = .2f;
    [SerializeField] private GameObject continueButton;

    private MakeNarration narrationMaker;
    private Narrative narrative;
    private int sentencesCount = 0;
    private int index = 0;

    private void Awake()
    {
        narrationMaker = FindObjectOfType<MakeNarration>();
    }

    private void OnEnable()
    {
        if (narrative != null)
        {
            sentencesCount = narrative.sentence.Length;
            NextSentence();
        }
    }

    public void NextSentence()
    {
        if (narrative != null && index < sentencesCount)
        {
            continueButton.SetActive(false);
            narrationText.text = string.Empty;
            StartCoroutine(Type());
            index++;
        }
        else
        {
            narrationMaker.HideNarrativeSystem();
        }
    }

    private IEnumerator Type()
    {
        foreach (char letter in narrative.sentence[index].ToCharArray())
        {
            narrationText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (sentencesCount >= index)
        {
            continueButton.SetActive(true);
        }
    }

    private void OnDisable()
    {
        index = 0;
        OnNarrationHide?.Invoke(this, EventArgs.Empty);
    }

    public void SetNarrative(Narrative narrative)
    {
        this.narrative = narrative;
    }

    public void MakeItFaster()
    {
        typingSpeed += 0.3f;
    }
    
}
