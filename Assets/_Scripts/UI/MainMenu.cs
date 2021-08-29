using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroupFader canvasFader;

    private bool requestedQuit;

    private void Awake()
    {
        GameEvents.OnGameStarted += FadeOutMenu;
        GameEvents.OnReturnToMainMenu += FadeInMenu;
    }

    private void FadeOutMenu() => canvasFader.RequestFadeOut();
    private void FadeInMenu() => canvasFader.RequestFadeIn();

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= FadeOutMenu;
        GameEvents.OnReturnToMainMenu -= FadeInMenu;
    }

    public void Play()
    {
        if (!requestedQuit)
            GameEvents.RaiseOnGameStarted();
    }

    public void Quit()
    {
        if (!requestedQuit)
            StartCoroutine(QuitRoutine());
    }

    public void ReturnToMainMenu()
    {
        GameEvents.RaiseOnReturnToMainMenu();
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
