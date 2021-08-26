using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    [SerializeField] private string pieceFolderPath;

    private List<GameObject> pieces;
    private int currentPiece;

    private void Start()
    {
        pieces = new List<GameObject>(Resources.LoadAll<GameObject>(pieceFolderPath));
        pieces.Shuffle();
    }

    public GameObject GenerateRandomPiece()
    {
        var piece = Instantiate(pieces[currentPiece++]);
        if (currentPiece == pieces.Count)
        {
            pieces.Shuffle();
            currentPiece = 0;
        }
        return piece;
    }
}
