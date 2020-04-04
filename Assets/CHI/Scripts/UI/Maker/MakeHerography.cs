using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeHerography : MonoBehaviour
{

    [Header("Frame")]
    [SerializeField] private Image frame;
    [SerializeField] Sprite characterFrame;

    [Header("Manager")]
    [SerializeField] private GameObject herographyManager;

    [Header("Content")]
    [SerializeField] private HeroBiography heroBiography;

    [Header("SFX")]
    [SerializeField] private AudioClip onClickSFX;
        
    private Herography herography;

    private void Awake()
    {
        herography = herographyManager.GetComponent<Herography>();
    }

    private void Start()
    {
        frame.sprite = characterFrame;
    }

    public void Make()
    {

        AudioSource.PlayClipAtPoint(onClickSFX, Camera.main.transform.position);
        herography.SetAvatar(heroBiography.avatar);
        herography.SetAlias(heroBiography.alias);
        herography.SetStory(heroBiography.story);

        herographyManager.SetActive(true);
    }
}
