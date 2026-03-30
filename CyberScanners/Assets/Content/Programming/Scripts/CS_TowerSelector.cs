using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSelectorWorld : MonoBehaviour
{
    public Camera cam;
    public TowerUI towerUI;
    public Color rangeColor = Color.green; // color of the range ring
    public GameObject rangeIndicatorPrefab;
    private GameObject currentRangeIndicator;
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
        if (currentSelected != null && currentRangeIndicator != null)
        {
            UpdateRangeIndicator();
        }
    }

    void SelectTower(Tower tower)
    {
        if (currentSelected != null)
            currentSelected.SetOutline(false);

        // destroy old range indicator
        if (currentRangeIndicator != null)
            Destroy(currentRangeIndicator);

        currentSelected = tower;

        currentSelected.SetOutline(true);

        // spawn new range indicator
        currentRangeIndicator = Instantiate(rangeIndicatorPrefab, tower.transform.position, Quaternion.identity);

        // apply scale based on range
        UpdateRangeIndicator();

        if (towerUI != null)
            towerUI.ShowTower(tower);
    }

    void UpdateRangeIndicator()
    {
        if (currentSelected == null || currentRangeIndicator == null) return;

        float diameter = currentSelected.range * 2f;

        currentRangeIndicator.transform.position =
            currentSelected.transform.position + new Vector3(0f, 0.1f, 0f);

        currentRangeIndicator.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);

        currentRangeIndicator.transform.localScale =
            new Vector3(diameter, diameter, diameter);
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