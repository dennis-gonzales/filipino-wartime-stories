using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGuide : MonoBehaviour
{
    [Header("Guide")]
    [SerializeField] private GameObject hudGuide;
    [SerializeField] private float timeToHide = 10;

    private HelpHandler helpHandler;

    private void Awake()
    {
        helpHandler = hudGuide.GetComponent<HelpHandler>();
        helpHandler.ShowGuide();
        helpHandler.hidden = false;

    }
}
