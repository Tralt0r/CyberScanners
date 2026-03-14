using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSelectorWorld : MonoBehaviour
{
    public Camera cam;
    public TowerUI towerUI;
    public Color rangeColor = Color.green; // color of the range ring

    private Tower currentSelected;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Tower tower = hit.collider.GetComponentInParent<Tower>();

                if (tower != null)
                {
                    SelectTower(tower);
                }
            }
        }
    }

    void SelectTower(Tower tower)
    {
        if (currentSelected != null)
            currentSelected.SetOutline(false);

        currentSelected = tower;

        currentSelected.SetOutline(true);

        if (towerUI != null)
            towerUI.ShowTower(tower);
    }

    void OnDrawGizmos()
    {
        if (currentSelected != null)
        {
            Gizmos.color = rangeColor;
            // Draw a flat circle at the tower's position
            Gizmos.DrawWireSphere(currentSelected.transform.position, currentSelected.range);
        }
    }
}