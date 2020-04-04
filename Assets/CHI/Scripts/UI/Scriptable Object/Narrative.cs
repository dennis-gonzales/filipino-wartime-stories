using UnityEngine;

[CreateAssetMenu(fileName = "New Narrative", menuName = "FWS/New Narrative")]
public class Narrative : ScriptableObject
{

    [TextArea(4, 8)]
    public string[] sentence;
}
