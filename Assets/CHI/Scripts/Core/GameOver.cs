using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private AsyncOperation operation;

    public void ReloadScene()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadAsynchronously(int buildIndex)
    {
        operation = SceneManager.LoadSceneAsync(buildIndex);

        while (!operation.isDone)
        {
            yield return null;
        }

    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);

        StartCoroutine(LoadAsynchronously(0));
    }
}
