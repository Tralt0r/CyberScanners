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

    void OnEnable()
    {
        EconomySystem.OnDataChanged += UpdateDataText;
        EconomySystem.OnInsufficientFunds += FlashInsufficientFunds;
    }

    void OnDisable()
    {
        EconomySystem.OnDataChanged -= UpdateDataText;
        EconomySystem.OnInsufficientFunds -= FlashInsufficientFunds;
    }

    void Update()
    {
        if (progression != null)
            waveText.text = "Wave: " + progression.currentWave;

        if (core != null)
            coreHealthText.text = "Core Health: " + core.currentHealth;
    }

    private void UpdateDataText(int newAmount)
    {
        dataText.text = "Data: " + newAmount;
    }

    private void FlashInsufficientFunds()
    {
        Debug.Log("[UI] Not enough data!");
    }
}