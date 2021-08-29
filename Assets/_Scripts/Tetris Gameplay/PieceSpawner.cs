using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private string pieceFolderPath;

    private List<GameObject> pieces;
    private int pieceIndex;

    private void Start()
    {
        GameEvents.OnGameStarted += StartSpawning;
        GameEvents.OnGameEnded += StopSpawning;
        GameEvents.OnReturnToMainMenu += StopSpawning;

        pieces = new List<GameObject>(Resources.LoadAll<GameObject>(pieceFolderPath));
        pieces.Shuffle();
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= StartSpawning;
        GameEvents.OnGameEnded -= StopSpawning;
        GameEvents.OnReturnToMainMenu -= StopSpawning;
    }

    private void StartSpawning()
    {
        StartCoroutine(PieceSpawningRoutine());
    }

    private void StopSpawning()
    {
        StopAllCoroutines();
    }

    private IEnumerator PieceSpawningRoutine()
    {
        while (true)
        {
            var currentPiece = GenerateRandomPiece();
            yield return new WaitUntil(() => currentPiece == null);
        }
    }

    private GameObject GenerateRandomPiece()
    {
        var piece = Instantiate(pieces[pieceIndex++], transform.position, Quaternion.identity, transform);
        if (pieceIndex == pieces.Count)
        {
            pieces.Shuffle();
            pieceIndex = 0;
        }
        return piece;
    }
}
