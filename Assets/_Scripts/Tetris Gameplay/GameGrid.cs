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

    private int width, height;
    private Transform[,] grid;

    private void Awake()
    {
        instance = Singleton.Setup(this, instance) as GameGrid;
        width = Mathf.RoundToInt(gridBounds.size.x);
        height = Mathf.RoundToInt(gridBounds.size.y);
        grid = new Transform[width, height];
    }

    public bool IsPositionValid(Vector3 pos)
    {
        var gridPos = WorldPosToGridCoordinate(pos);
        if (grid[gridPos.x, gridPos.y] != null)
            return false;
        return pos.x >= gridBounds.min.x && pos.x <= gridBounds.max.x &&
               pos.y >= gridBounds.min.y;
    }

    public void AddCubeToGrid(Transform cube)
    {
        cube.SetParent(transform);
        var gridPos = WorldPosToGridCoordinate(cube.position);
        grid[gridPos.x, gridPos.y] = cube;
    }

    private Vector2Int WorldPosToGridCoordinate(Vector3 pos)
    {
        return new Vector2Int()
        {
            x = Mathf.Clamp(Mathf.FloorToInt(pos.x - gridBounds.min.x), 0, width - 1),
            y = Mathf.Clamp(Mathf.FloorToInt(pos.y - gridBounds.min.y), 0, height - 1)
        };
    }

    private void Update()
    {
        CheckForCompletedLines();
    }

    private void CheckForCompletedLines()
    {
        for (int i = height - 1; i >= 0; --i)
        {
            if (IsLineAtHeightCompleted(i))
            {
                ClearLineAtHeight(i);
                RollGridDownAtHeight(i);
            }
        }
    }

    bool IsLineAtHeightCompleted(int y)
    {
        for (int j = 0; j < width; ++j)
        {
            if (grid[j, y] == null)
                return false;
        }
        return true;
    }

    void ClearLineAtHeight(int y)
    {
        for (int j = 0; j < width; ++j)
        {
            Destroy(grid[j, y].gameObject);
            grid[j, y] = null;
        }
    }

    void RollGridDownAtHeight(int y)
    {
        for (int i = y; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (grid[j, i] != null)
                {
                    grid[j, i].position += Vector3.down;
                    grid[j, i - 1] = grid[j, i];
                    grid[j, i] = null;
                }
            }
        }
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
