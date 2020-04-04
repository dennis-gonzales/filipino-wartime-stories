using UnityEngine;

public class HelpHandler : MonoBehaviour
{
    [Header("Help")]
    [SerializeField] private GameObject[] helpObject;

    public bool hidden = true;

    public void ToggleGuide()
    {
        hidden = !hidden;

        if (hidden)
        {
            HideGuide();
        }
        else
        {
            ShowGuide();
        }

        
    }

    public void ShowGuide()
    {
        if (helpObject != null)
        {
            foreach (var item in helpObject)
            {
                item.SetActive(true);
            }
        }
    }

    public void HideGuide()
    {
        if (helpObject != null)
        {
            foreach (var item in helpObject)
            {
                item.SetActive(false);
            }
        }
    }

}
