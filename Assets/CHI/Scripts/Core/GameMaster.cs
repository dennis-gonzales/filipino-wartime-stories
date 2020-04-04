using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private const string _playerTag = "Player";
    [Header("Debug")]
    [Range(0, 1)] [SerializeField] private float timeScale = 1;

    [Header("Main Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("BGM")]
    [SerializeField] private AudioClip audioBGM;

    [Header("UI")]
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject dialogueManager;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pauseManager;

    private DialogueSystem dialogueSystem;

    public Vector2 lastCheckPointPos;

    private CharacterController2D playerController;

    private AsyncOperation operation;

    AudioSource cameraAudioSource;

    public static bool paused = false;

    private void Awake()
    {
        Time.timeScale = 1;

        dialogueSystem = dialogueManager.GetComponent<DialogueSystem>();
        playerController = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<CharacterController2D>();
        playerController.OnPlayerDies += PlayerController_OnPlayerDies;

        cameraAudioSource = Camera.main.GetComponent<AudioSource>();

        if (audioBGM != null)
        {
            cameraAudioSource.clip = audioBGM;
            cameraAudioSource.Play();
        }
    }

    private void PlayerController_OnPlayerDies(object sender, EventArgs e)
    {
        if (!SceneManager.GetActiveScene().name.Equals("ACT1_Miserable"))
        {
            if (!SceneManager.GetActiveScene().name.Equals("ACT2_Escape"))
            {
                if (!SceneManager.GetActiveScene().name.Equals("ACT2_Original"))
                {
                    if (!SceneManager.GetActiveScene().name.Equals("ACT3_Miserable"))
                    {
                        if (!SceneManager.GetActiveScene().name.Equals("ACT3_Original"))
                        {
                            if (gameOver != null)
                            {
                                gameOver.SetActive(true);
                            }
                        }
                    }
                }
            }

        }
    }

    public void TogglePause()
    {

        paused = !paused;

        if (paused)
        {
            Pause();
        }
        else
        {
            Resume();
        }


    }

    public void Pause()
    {
        if (pauseManager != null)
        {
            Time.timeScale = 0;
            pauseManager.SetActive(true);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseManager.SetActive(false);
    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        StartCoroutine(LoadAsynchronously(0));
    }


    private IEnumerator LoadAsynchronously(int scene)
    {
        operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        //TODO: remove on release
        Time.timeScale = timeScale;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
        }
    }

    public void ShowDialogueSystem()
    {
        if (dialogueManager != null)
        {
            dialogueManager.SetActive(true);
        }
    }

    public void HideDialogueSystem()
    {
        if (dialogueManager != null)
        {
            dialogueManager.SetActive(false);
        }
    }

    public void MakeDialog(Dialogue dialogue)
    {
        dialogueSystem.SetDialogue(dialogue);
        ShowDialogueSystem();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
