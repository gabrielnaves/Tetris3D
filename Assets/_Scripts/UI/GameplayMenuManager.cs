using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroupFader canvasFader;

    private void Awake()
    {
        GameEvents.OnGameStarted += FadeInCanvas;
    }

    private void FadeInCanvas() => canvasFader.RequestFadeIn();

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= FadeInCanvas;
    }
}
