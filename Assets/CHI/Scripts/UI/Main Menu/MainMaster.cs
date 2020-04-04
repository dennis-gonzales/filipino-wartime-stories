using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMaster : MonoBehaviour
{
    [Header("New Game")]
    [SerializeField] private GameObject newGamePanel;

    [Header("Loading Bar")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI progressText;

    private AsyncOperation operation;
    private float progress;
    private int sceneIndex;

    public void ShowNewGame()
    {
        newGamePanel.SetActive(true);
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("CurrentScene", 1);
        StartCoroutine(LoadAsynchronously(1));
    }

    public void Continue()
    {
        sceneIndex = PlayerPrefs.GetInt("CurrentScene", 1);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadLevelChooser(int i)
    {
        StartCoroutine(LoadAsynchronously(i));
    }

    private IEnumerator LoadAsynchronously(int scene)
    {
        operation = SceneManager.LoadSceneAsync(scene);

        slider.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = Mathf.Round(progress * 100) + "%";
            yield return null;

        }

    }

    public void Quit()
    {
        Application.Quit();
    }

    public Slider GetSlider() => slider;

    public TextMeshProUGUI GetProgressText() => progressText;

}
