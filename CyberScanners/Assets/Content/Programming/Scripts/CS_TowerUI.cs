using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class TowerUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Text Fields")]
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text rangeText;
    public TMP_Text levelText;
    public TMP_Text sellText;
    public TMP_Text pathText;
    public TMP_Text descriptionText;
    public TMP_Text upgradeCostText;

    private Tower currentTower;

    void Start()
    {
        panel.SetActive(false);
    }
    public void ShowTower(Tower tower)
    {
        currentTower = tower;
        panel.SetActive(true);

        nameText.text = tower.towerName;
        healthText.text = "Health: " + tower.currentHealth + "/" + tower.maxHealth;
        damageText.text = "Damage: " + tower.damage;
        rangeText.text = "Range: " + tower.range;
        levelText.text = "Level: " + tower.upgradeLevel;
        sellText.text = "Sell Value: " + tower.sellValue;
        pathText.text = "Path: " + tower.upgradePath;
        descriptionText.text = tower.description;
        upgradeCostText.text = "Upgrade Cost: " + tower.upgradeLevels[tower.upgradeLevel - 1].upgradeCost;
    }

    public void SellTower()
    {
        if (currentTower == null) return;

        currentTower.Sell();
        panel.SetActive(false);
    }

    public void Close()
    {
        panel.SetActive(false);
        currentTower = null;
    }

    public void Open()
    {
        if (currentTower != null)
            panel.SetActive(true);
    }
    public void UpgradeTower()
    {
        if (currentTower == null) return;

        currentTower.Upgrade();
        ShowTower(currentTower);

        sellText.text = "Sell Value: " + currentTower.sellValue;
        upgradeCostText.text = "Upgrade Cost: " + currentTower.upgradeLevels[currentTower.upgradeLevel - 1].upgradeCost;
    }
}