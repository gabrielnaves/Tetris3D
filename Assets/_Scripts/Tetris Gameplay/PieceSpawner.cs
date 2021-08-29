using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private string pieceFolderPath;

    private List<GameObject> pieces;
    private int currentPiece;

    private void Start()
    {
        pieces = new List<GameObject>(Resources.LoadAll<GameObject>(pieceFolderPath));
        pieces.Shuffle();
        StartSpawning();
    }

    public void StartSpawning()
    {
        StartCoroutine(PieceSpawningRoutine());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    IEnumerator PieceSpawningRoutine()
    {
        while (true)
        {
            var currentPiece = GenerateRandomPiece();
            yield return new WaitUntil(() => currentPiece == null);
        }
    }

    private GameObject GenerateRandomPiece()
    {
        var piece = Instantiate(pieces[currentPiece++], transform.position, Quaternion.identity, transform);
        if (currentPiece == pieces.Count)
        {
            pieces.Shuffle();
            currentPiece = 0;
        }
        return piece;
    }
}
