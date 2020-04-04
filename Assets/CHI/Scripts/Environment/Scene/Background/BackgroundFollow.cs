using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [Header("Background Offset")]
    [SerializeField] private float X_offset = 0;

    [Header("BG Scroller")]
    [SerializeField] private GameObject backgroundNear;
    [SerializeField] private GameObject backgroundFar;

    private BackgroundScroller bgNear;
    private BackgroundScroller bgFar;

    private LapuLapu hero;
    private Vector3 oldPosition = Vector2.zero;
    private Vector3 newPosition = Vector2.zero;

    private float speed = 0;


    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        hero = FindObjectOfType<LapuLapu>();
        oldPosition = transform.position;

        bgNear = backgroundNear.GetComponent<BackgroundScroller>();
        bgFar = backgroundFar.GetComponent<BackgroundScroller>();


    }

    private void Start()
    {
        FollowPlayer();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        ScrollBackground();
    }

    private void FollowPlayer()
    {
        newPosition = oldPosition;

        newPosition.x = transform.position.x;
        newPosition.x += X_offset;

        newPosition.y = bgNear.transform.position.y;
        newPosition.z = bgNear.transform.position.z;
        bgNear.transform.position = newPosition;

        newPosition.y = bgFar.transform.position.y;
        newPosition.z = bgFar.transform.position.z;
        bgFar.transform.position = newPosition;
    }

    private void ScrollBackground()
    {
        speed = (oldPosition.x - newPosition.x) * Time.fixedDeltaTime;

        bgNear.SetScrollSpeed(speed * -0.45f);
        bgFar.SetScrollSpeed(speed * -0.3f);
    }
}
