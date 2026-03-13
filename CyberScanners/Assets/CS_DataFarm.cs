using System.Collections;
using UnityEngine;

public class CS_DataFarm : MonoBehaviour
{
    public int level = 1;
    public EconomySystem economy;
    public Tower tower; // Reference to the tower that owns this data farm
    public int dataPerInterval = 10; // How much cash to add each interval
    public float dataFrequency = 5f; // Seconds between adding cash

    private Coroutine cashRoutine;

    void Start()
    {
        if (economy == null)
            economy = Object.FindFirstObjectByType<EconomySystem>();

        // Start the cash generation loop
        cashRoutine = StartCoroutine(GenerateCashLoop());
    }

    void Update()
    {
        // Keep level in sync with the tower
        if (tower != null)
            level = tower.upgradeLevel;

        // Update cash values based on level
        UpdateCashValues();
    }

    void UpdateCashValues()
    {
        switch (level)
        {
            case 1:
                dataFrequency = 10f;
                dataPerInterval = 10;
                break;
            case 2:
                dataFrequency = 7f;
                dataPerInterval = 20;
                break;
            case 3:
                dataFrequency = 5f;
                dataPerInterval = 30;
                break;
            case 4:
                dataFrequency = 3f;
                dataPerInterval = 50;
                break;
            case 5:
                dataFrequency = 1.5f;
                dataPerInterval = 100;
                break;
            case 6:
                dataFrequency = 0.5f;
                dataPerInterval = 200;
                break;
            default:
                dataFrequency = 10f;
                dataPerInterval = 10;
                break;
        }
    }

    private IEnumerator GenerateCashLoop()
    {
        while (true)
        {
            economy.AddData(dataPerInterval);
            yield return new WaitForSeconds(dataFrequency);
        }
    }
}