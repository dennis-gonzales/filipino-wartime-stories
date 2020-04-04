using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] [Range(5, 6)] private int maxZoomIn = 5;
    [SerializeField] [Range(7, 8)] private int maxZoomOut = 8;

    private CinemachineVirtualCamera CV_Camera;

    private Touch touchZero;
    private Touch touchOne;

    private Vector2 touchZeroPrevPos;
    private Vector2 touchOnePrevPos;

    private float prevMagnitude;
    private float currentMagnitude;
    private float difference;

    private void Awake()
    {
        CV_Camera = GetComponent<CinemachineVirtualCamera>();
    }

    private void FixedUpdate()
    {

        if(Input.touchCount == 2)
        {
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);

            touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }

        //Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void Zoom(float zoom)
    {
        CV_Camera.m_Lens.OrthographicSize = Mathf.Clamp(
            CV_Camera.m_Lens.OrthographicSize - zoom,
            maxZoomIn, maxZoomOut
        );
    }
}
