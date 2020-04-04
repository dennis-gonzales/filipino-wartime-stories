using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private Dialogue inspectNothing;
    

    private GameMaster gm;
    private Chest chest;
    private Switch switchs;
    private Checkpoint checkpoint;
    private Door door;

    private bool key = false;
    private bool search = false;
    private bool contact = false;

    private void Awake()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    public void Pickup()
    {
        search = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (search)
        {
            checkpoint = collision.GetComponent<Checkpoint>();
            chest = collision.GetComponent<Chest>();
            door = collision.GetComponent<Door>();
            switchs = collision.GetComponent<Switch>();

            if (checkpoint != null)
            {
                checkpoint.Inspect();
            }

            if (chest != null)
            {
                chest.Unlock();
            }
            
            if (door != null)
            {
                door.Unlock(key);
            }

            if (switchs != null)
            {
                switchs.Unlock();
            }
        }

        search = false;
        contact = true;
    }

    private void FixedUpdate()
    {
        contact = false;
    }

    private void Update()
    {
        if (!contact)
        {

            if (inspectNothing != null && search)
            {
                gm.MakeDialog(inspectNothing);
            }

            search = false;
        }
    }

    public void SetKey(bool key)
    {
        this.key = key;
    }

}
