using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    public GridPlacement gridPlacement;

    public void SelectTower(GameObject towerPrefab)
    {
        gridPlacement.selectedTowerPrefab = towerPrefab;
    }
}