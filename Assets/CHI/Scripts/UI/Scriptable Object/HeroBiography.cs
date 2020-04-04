using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Herography", menuName = "FWS/New Herography")]
public class HeroBiography : ScriptableObject
{
    public Sprite avatar;
    public string alias;
    [TextArea(4, 8)] public string story;
}
