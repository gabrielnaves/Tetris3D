using UnityEngine;

public class GameplayMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroupFader canvasFader;

    private void Awake()
    {
        GameEvents.OnGameStarted += FadeInCanvas;
        GameEvents.OnReturnToMainMenu += FadeOutCanvas;
    }

    private void FadeInCanvas() => canvasFader.RequestFadeIn();
    private void FadeOutCanvas() => canvasFader.RequestFadeOut();

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= FadeInCanvas;
        GameEvents.OnReturnToMainMenu -= FadeOutCanvas;
    }
}
