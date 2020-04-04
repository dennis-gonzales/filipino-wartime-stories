using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelChooser : MonoBehaviour
{
    [Header("Object to hide")]
    [SerializeField] private GameObject[] toHide;


    private MainMaster mm;

    private AsyncOperation operation;
    private float progress;
    private int sceneIndex;

    private void Awake()
    {
        mm = FindObjectOfType<MainMaster>();
    }

    public void LoadLevel(int sceneIndex)
    {
        mm.LoadLevelChooser(sceneIndex);
        gameObject.SetActive(false);

        if (toHide != null)
        {
            foreach (var item in toHide)
            {
                item.SetActive(false);
            }
        }
    }
}
