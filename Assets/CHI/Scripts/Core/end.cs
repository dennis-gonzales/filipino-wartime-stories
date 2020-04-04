using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour
{
    [Header("Select level")]
    [SerializeField] private LoadLevel loadLevel = LoadLevel.ACT3_Intro_02;

    private const string _playerTag = "Player";
    
    private MakeNarration narrationMaker;
    private MakeChoice makeChoice;

    private AsyncOperation operation;

    private enum LoadLevel
    {
        ACT3_Intro_02,
        predestined
    }

    private void Awake()
    {
        narrationMaker = FindObjectOfType<MakeNarration>();
        makeChoice = FindObjectOfType<MakeChoice>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(_playerTag))
        {
            Time.timeScale = 0;

            if (narrationMaker != null)
            {
                narrationMaker.PlayEnding();
                narrationMaker.GetNarrativeSystem().OnNarrationHide += playNextScene;
            }

            if (makeChoice != null)
            {
                makeChoice.ChoiceMaker();
            }
        }
    }

    private void playNextScene(object sender, EventArgs e)
    {
        switch (loadLevel)
        {
            case LoadLevel.ACT3_Intro_02:
                StartCoroutine(LoadAsynchronously(loadLevel.ToString()));
                break;
            case LoadLevel.predestined:

                string destiny = PlayerPrefs.GetString("Consequence", "miserable");

                if (destiny.Equals("miserable"))
                {
                    StartCoroutine(LoadAsynchronously("ACT3_Miserable"));
                }
                else if (destiny.Equals("original"))
                {
                    StartCoroutine(LoadAsynchronously("ACT3_Original"));
                }

                break;
        }
    }

    private IEnumerator LoadAsynchronously(string scene)
    {
        operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            yield return null;

        }

    }
}
