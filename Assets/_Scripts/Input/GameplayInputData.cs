using UnityEngine;

[CreateAssetMenu(menuName = "GameplayInputData")]
public class GameplayInputData : ScriptableObject
{
    public float Movement { get; set; }
    public float MovementDown { get; set; }
    public bool Rotate { get; set; }
    public bool Fall { get; set; }
    public bool Skip { get; set; }

    public void Reset()
    {
        Movement = 0;
        MovementDown = 0;
        Rotate = false;
        Fall = false;
        Skip = false;
    }
}
