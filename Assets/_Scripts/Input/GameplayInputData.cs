using UnityEngine;

[CreateAssetMenu(menuName = "GameplayInputData")]
public class GameplayInputData : ScriptableObject
{
    public float Movement { get; set; }
    public bool Rotate { get; set; }
    public bool Fall { get; set; }
    public bool Skip { get; set; }
}