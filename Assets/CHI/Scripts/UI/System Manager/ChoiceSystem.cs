using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChoiceSystem : MonoBehaviour
{
    [Header("Choises Obj")]
    [SerializeField] private TextMeshProUGUI choiceObj_1;
    [SerializeField] private TextMeshProUGUI choiceObj_2;

    [Header("Choises")]
    [SerializeField] private string choiceText_1;
    [SerializeField] private string choiceText_2;

    [Header("Choice Destination")]
    [SerializeField] private Levels choice_1;
    [SerializeField] private Levels choice_2;

    [Header("Loading Bar")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI progressText;

    private AsyncOperation operation;
    private float progress;

    private enum Levels
    {
        MainMenu,
        ACT1_Intro,
        ACT1_Miserable,
        ACT1_Original_01,
        ACT1_Original_02,
        ACT2_Intro_01,
        ACT2_Intro_02,
        ACT2_Escape,
        ACT2_Original,
        ACT3_Intro_01,
        ACT3_Intro_02,
        ACT3_Miserable,
        ACT3_Original
    }

    private void Start()
    {
        choiceObj_1.text = choiceText_1;
        choiceObj_2.text = choiceText_2;
    }

    public void LoadLevel(int choice)
    {
        switch (choice)
        {

            case 1:
                StartCoroutine(LoadAsynchronously(choice_1));
                break;

            case 2:
                StartCoroutine(LoadAsynchronously(choice_2));
                break;
        }
    }

    private IEnumerator LoadAsynchronously(Levels level)
    {
        operation = SceneManager.LoadSceneAsync(level.ToString());

        slider.gameObject.SetActive(true);


        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = Mathf.Round(progress * 100) + "%";
            yield return null;

        }

    }
    
}
