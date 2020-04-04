using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scrolling")]
    [SerializeField] private float scrollSpeed = 0f;

    private Material material;
    private Vector2 offset;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        //material = GetComponent<RawImage>().material;
    }

    private void FixedUpdate()
    {
        offset = new Vector2(scrollSpeed, 0);
        material.mainTextureOffset = offset;
    }

    public void SetScrollSpeed(float x)
    {
        scrollSpeed = x;
    }
}
