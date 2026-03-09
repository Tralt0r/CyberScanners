using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 6;
    public float cellSize = 1f;

    public bool[,] occupied;
    public bool[,] pathTiles;

    [Header("Optional: Automatic path detection")]
    public EnemyPath enemyPath;

    void Awake()
    {
        occupied = new bool[gridWidth, gridHeight];
        pathTiles = new bool[gridWidth, gridHeight];

        if (enemyPath != null)
        {
            MarkPathTiles();
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return transform.position + new Vector3(x * cellSize, 0, y * cellSize);
    }

    public bool IsOccupied(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) return true;
        return occupied[x, y];
    }

    public void SetOccupied(int x, int y, bool value)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) return;
        occupied[x, y] = value;
    }

    public bool IsPathTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) return true;
        return pathTiles[x, y];
    }

    public void SetPathTile(int x, int y, bool value)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) return;
        pathTiles[x, y] = value;
    }

    public void MarkPathTiles()
    {
        if (enemyPath == null || enemyPath.waypoints.Count == 0) return;

        for (int i = 0; i < enemyPath.waypoints.Count; i++)
        {
            Vector3 wp = enemyPath.waypoints[i].position;
            int x = Mathf.FloorToInt((wp.x - transform.position.x) / cellSize);
            int y = Mathf.FloorToInt((wp.z - transform.position.z) / cellSize);

            SetPathTile(x, y, true);

            if (i < enemyPath.waypoints.Count - 1)
            {
                Vector3 nextWp = enemyPath.waypoints[i + 1].position;
                FillLine(x, y, nextWp);
            }
        }
    }

    void FillLine(int startX, int startY, Vector3 endPos)
    {
        int endX = Mathf.FloorToInt((endPos.x - transform.position.x) / cellSize);
        int endY = Mathf.FloorToInt((endPos.z - transform.position.z) / cellSize);

        int dx = Mathf.Abs(endX - startX);
        int dy = Mathf.Abs(endY - startY);
        int sx = startX < endX ? 1 : -1;
        int sy = startY < endY ? 1 : -1;
        int err = dx - dy;

        int x = startX;
        int y = startY;

        while (true)
        {
            SetPathTile(x, y, true);
            if (x == endX && y == endY) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x += sx; }
            if (e2 < dx) { err += dx; y += sy; }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Handles.color = Color.gray;

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = origin + new Vector3(x * cellSize, 0, 0);
            Vector3 end = origin + new Vector3(x * cellSize, 0, gridHeight * cellSize);
            Handles.DrawAAPolyLine(2f, start, end);
        }

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = origin + new Vector3(0, 0, y * cellSize);
            Vector3 end = origin + new Vector3(gridWidth * cellSize, 0, y * cellSize);
            Handles.DrawAAPolyLine(2f, start, end);
        }
    }
#endif
}