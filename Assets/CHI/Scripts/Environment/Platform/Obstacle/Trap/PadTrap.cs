using UnityEngine;

public class PadTrap : MonoBehaviour
{
    private const string _playerTag = "Player";

    [SerializeField] private int minTrapDamage = 10;
    [SerializeField] private int maxTrapDamage = 20;
    [Range(.1f, 2f)] [SerializeField] private float damageOverTime = 1;

    private float counter = 0;

    private CharacterController2D player;
    private int playerHealth;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<CharacterController2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (counter <= 0)
        {
            if (collision.gameObject.CompareTag(_playerTag))
            {

                playerHealth = player.GetCurrentHealth();

                if (playerHealth > 0)
                {
                    player.Trap(Random.Range(minTrapDamage, maxTrapDamage));
                }
                else
                {
                    // player died in trap
                    // TODO: add UI for this
                }
                counter = damageOverTime;
            }
        }

        counter -= Time.deltaTime;
    }
}
