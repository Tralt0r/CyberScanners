using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("TMP Text References")]
    public TMP_Text waveText;
    public TMP_Text dataText;
    public TMP_Text coreHealthText;

    [Header("System References")]
    public ProgressionSystem progression;
    public EconomySystem economy;
    public CoreSystem core;

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (progression != null)
            waveText.text = "Wave: " + progression.currentWave;

        if (economy != null)
            dataText.text = "Data: " + economy.currentData;

        if (core != null)
            coreHealthText.text = "Core Health: " + core.currentHealth;
    }
}