using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroupFader canvasFader;

    private bool requestedQuit;

    private void Awake()
    {
        GameEvents.OnGameStarted += FadeOutMenu;
    }

    private void FadeOutMenu() => canvasFader.RequestFadeOut();

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= FadeOutMenu;
    }

    public void Play()
    {
        GameEvents.RaiseOnGameStarted();
    }

    public void Quit()
    {
        if (!requestedQuit)
            StartCoroutine(QuitRoutine());
    }

    private IEnumerator QuitRoutine()
    {
        requestedQuit = true;
        yield return new WaitUntil(ScreenFader.instance.IsIdle);
        ScreenFader.instance.RequestFadeOut();
        yield return new WaitUntil(ScreenFader.instance.IsIdle);
        Application.Quit();
    }
}
