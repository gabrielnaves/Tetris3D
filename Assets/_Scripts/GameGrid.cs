using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    static public GameGrid instance { get; private set; }

    [SerializeField] private Bounds gridBounds;

    [Header("Debug")]
    [SerializeField] private bool showGizmos;
    [SerializeField] private Color gizmoColor;

    public bool IsPositionValid(Vector3 pos)
    {
        return pos.x >= gridBounds.min.x && pos.x <= gridBounds.max.x &&
               pos.y >= gridBounds.min.y;
    }

    private void Awake()
    {
        instance = Singleton.Setup(this, instance) as GameGrid;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(gridBounds.center, gridBounds.size);
        }
    }
}
