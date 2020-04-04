using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Vector3 scale;
    private float percentage;
    
    public void SetHealthPercentage(float percentage)
    {
        if (percentage >= 0f && percentage <= 1f)
        {
            scale = new Vector3(
            percentage,
            transform.localScale.y,
            transform.localScale.z);

            transform.localScale = scale;
        }
        else
        {
            Debug.LogWarning("Enemy health scale goes beyond 0 or 1");
        }
    }
    
}
