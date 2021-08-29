using UnityEngine;
using FMODUnity;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroupFader canvasFader;
    [SerializeField, EventRef] private string gameOverSound;

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
        RuntimeManager.PlayOneShot(gameOverSound);
        canvasFader.RequestFadeIn(true);
    }
}
