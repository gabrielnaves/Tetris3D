using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    static public GameScore instance { get; private set; }

    [SerializeField] private int pointsPerLine = 100;

    [Header("Debug")]
    public int score;

    private void Awake()
    {
        instance = Singleton.Setup(this, instance) as GameScore;

        GameEvents.OnGameStarted += ResetScore;
        GameEvents.OnLineCleared += IncreaseScore;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= ResetScore;
        GameEvents.OnLineCleared -= IncreaseScore;
    }

    private void ResetScore() => score = 0;
    private void IncreaseScore() => score += pointsPerLine;
}
