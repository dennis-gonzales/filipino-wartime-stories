using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint")]
    [Range(0, 5)] [SerializeField] private int checkpointDelay = 0;

    [Header("Dialogue")]
    [SerializeField] Dialogue checkpointReached;

    private const string targetPlayer = "Player";

    private GameMaster gm;

    private bool checkpointSaved = false;

    private void Awake()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetPlayer))
        {
            StartCoroutine(NewCheckPointPosition());
        }
    }

    IEnumerator NewCheckPointPosition()
    {
        yield return new WaitForSeconds(checkpointDelay);
        
        if (gm != null)
        {
            checkpointSaved = true;
            gm.lastCheckPointPos = transform.position;
        }
        else
        {
            Debug.LogWarning("Game master object couldn't be found.");
        }
    }

    public void Inspect()
    {
        if (checkpointSaved)
        {
            gm.MakeDialog(checkpointReached);
        }
    }
}
