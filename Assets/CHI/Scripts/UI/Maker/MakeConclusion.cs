using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeConclusion : MonoBehaviour
{
    private const string _playerTag = "Player";
    [Header("Conclusion Manager")]
    [SerializeField] private GameObject conclusionManager;
    
    [Header("Sprite")]
    [SerializeField] private Sprite winner;
    [SerializeField] private Sprite loser;

    [Header("Conclusion Content")]
    [TextArea(3, 5)]
    [SerializeField] private string conclusionText;

    [Header("Scene Manager")]
    [SerializeField] private string nextLevel;

    [Header("Conclusion what?")]
    [SerializeField] private ConclusionType conclusionType = ConclusionType.Original;

    private enum ConclusionType
    {
        Original,
        Miserable
    }

    private AsyncOperation operation;
    private float progress;

    private ConclusionSystem conclusionSystem;

    private CharacterController2D playerController;
    private Enemy magellan;

    private void Awake()
    {
        conclusionSystem = conclusionManager.GetComponent<ConclusionSystem>();

        conclusionSystem.SetWinnerSprite(winner);
        conclusionSystem.SetLoserSprite(loser);
        conclusionSystem.SetConclusionText(conclusionText);
    }

    private void Start()
    {
        switch (conclusionType)
        {
            case ConclusionType.Original:
                magellan = FindObjectOfType<Enemy>();
                magellan.OnEnemyDies += MagellanDied;
                break;

            case ConclusionType.Miserable:
                playerController = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<CharacterController2D>();
                playerController.OnPlayerDies += PlayerDied;
                    break;
        }
    }

    private void PlayerDied(object sender, EventArgs e)
    {
        if (conclusionManager != null)
        {
            conclusionManager.SetActive(true);
        }
    }

    private void MagellanDied(object sender, EventArgs e)
    {
        if (conclusionManager != null)
        {
            conclusionManager.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        if (nextLevel != null)
        {
            StartCoroutine(LoadAsynchronously(nextLevel));
        }
    }

    private IEnumerator LoadAsynchronously(string scene)
    {

        operation = SceneManager.LoadSceneAsync(scene);

        conclusionSystem.GetSlider.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            conclusionSystem.GetSlider.value = progress;
            conclusionSystem.GetProgressText.text = Mathf.Round(progress * 100) + "%";
            yield return null;

        }
    }
}
