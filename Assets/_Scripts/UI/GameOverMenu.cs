using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroupFader canvasFader;

    private void Awake()
    {
        GameEvents.OnGameEnded += OnGameEnded;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameEnded -= OnGameEnded;
    }

    private void OnGameEnded()
    {
        canvasFader.RequestFadeIn(true);
    }
}
