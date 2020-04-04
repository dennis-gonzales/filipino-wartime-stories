using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "FWS/New Dialogue")]
public class Dialogue : ScriptableObject
{

    [TextArea(2, 4)]
    public string[] sentence;
}
