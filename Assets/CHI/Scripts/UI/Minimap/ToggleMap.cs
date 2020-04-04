using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMap : MonoBehaviour
{
    private bool isMapVisible = true;

    public void Toggle()
    {
            isMapVisible = !isMapVisible;
            gameObject.SetActive(isMapVisible);
        
    }
}
