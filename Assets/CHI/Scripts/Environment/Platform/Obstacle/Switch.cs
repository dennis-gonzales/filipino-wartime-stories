using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    [Header("Switch")]
    [SerializeField] private Sprite redSwitch;
    [SerializeField] private SpriteRenderer switchSprite;

    [Header("Dialogues")]
    [SerializeField] private Dialogue doorUnlocked;
    [SerializeField] private Dialogue doorOpened;

    private string _playerTag = "Player";

    private GameMaster gm;

    private GameObject playerObject;
    private Inspect playerInspect;

    private bool unclocked = false;

    private void Awake()
    {
        gm = FindObjectOfType<GameMaster>();

        playerObject = GameObject.FindGameObjectWithTag(_playerTag);
        playerInspect = playerObject.GetComponent<Inspect>();
    }

    public void Unlock()
    {

        if (unclocked)
        {
            gm.MakeDialog(doorUnlocked);
        }

        else
        {
            gm.MakeDialog(doorOpened);
            unclocked = true;
            switchSprite.sprite = redSwitch;
            playerInspect.SetKey(true);
        }
    }
}
