using UnityEngine;
using UnityEngine.InputSystem;

public class GridPlacement : MonoBehaviour
{
    [Header("References")]
    public GridSystem grid;
    public GameObject towerPrefab;
    public Camera activeCamera;

    [Header("Colors")]
    public Color hoverColor = new Color(1f, 1f, 0f, 0.3f);
    public Color invalidColor = new Color(1f, 0f, 0f, 0.3f);

    private Vector2Int currentTile;
    private bool isValidTile;

    void Update()
    {
        if (grid == null || activeCamera == null || grid.occupied == null) return;

        UpdateMouseTile();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlaceTower();
        }
    }

    void UpdateMouseTile()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = activeCamera.ScreenToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            activeCamera.transform.position.y - grid.transform.position.y
        ));

        int x = Mathf.FloorToInt((worldPos.x - grid.transform.position.x) / grid.cellSize);
        int y = Mathf.FloorToInt((worldPos.z - grid.transform.position.z) / grid.cellSize);

        currentTile = new Vector2Int(x, y);

        if (x >= 0 && y >= 0 && x < grid.gridWidth && y < grid.gridHeight)
        {
            isValidTile = !grid.IsOccupied(x, y) && !grid.IsPathTile(x, y);
        }
        else
        {
            isValidTile = false;
        }
    }

    void TryPlaceTower()
    {
        int x = currentTile.x;
        int y = currentTile.y;

        if (x < 0 || y < 0 || x >= grid.gridWidth || y >= grid.gridHeight) return;

        if (!isValidTile) return;

        Vector3 pos = grid.GetWorldPosition(x, y) + new Vector3(grid.cellSize / 2f, 0, grid.cellSize / 2f);
        Instantiate(towerPrefab, pos, Quaternion.identity);

        grid.SetOccupied(x, y, true);
    }

    void OnDrawGizmos()
    {
        if (grid == null || grid.occupied == null) return;

        int x = currentTile.x;
        int y = currentTile.y;

        if (x < 0 || y < 0 || x >= grid.gridWidth || y >= grid.gridHeight) return;

        bool invalid = grid.IsOccupied(x, y) || grid.IsPathTile(x, y);
        Gizmos.color = invalid ? invalidColor : hoverColor;

        Vector3 pos = grid.GetWorldPosition(x, y) + new Vector3(grid.cellSize / 2f, 0, grid.cellSize / 2f);
        Gizmos.DrawCube(pos, Vector3.one * grid.cellSize);
    }
}