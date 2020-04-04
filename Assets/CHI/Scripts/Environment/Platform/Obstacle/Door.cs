using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Properties")]
    [SerializeField] private bool locked = true;

    [Header("Dialogue")]
    [SerializeField] private Dialogue doorLocked;
    [SerializeField] private Dialogue doorUnlocked;

    private GameMaster gm;
    private new BoxCollider2D collider;

    private void Awake()
    {
        gm = FindObjectOfType<GameMaster>();
        collider = GetComponent<BoxCollider2D>();
    }


    public void Unlock(bool key)
    {
        if (key)
        {
            locked = false;
            collider.enabled = false;
            gm.MakeDialog(doorUnlocked);
        }
        else
        {
            gm.MakeDialog(doorLocked);
        }
    }
}
