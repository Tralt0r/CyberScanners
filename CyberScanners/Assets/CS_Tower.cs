using UnityEngine;
using System.Collections.Generic;
using System;

public class Tower : MonoBehaviour
{

    public ProgressionSystem progressionSystem;
    public EnemySpawner enemySpawner;

    [Header("Stats")]
    public string towerName = "Basic Tower";
    public float maxHealth = 100;
    public float currentHealth;
    public bool ignoreDamageNerf;

    public int damage = 10;
    public float attackSpeed = 1f;
    public float range = 5f;

    [Header("Projectile")]
    public Projectile projectilePrefab;

    [Header("Upgrade")]
    public int upgradeLevel = 1;

    [Header("Economy")]
    public int buildCost = 100;
    public int sellValue = 50;

    [Header("Info")]
    [TextArea(2,4)]
    public string description = "Default tower description.";

    [Header("Pathway")]
    public string upgradePath = "Balanced";

    public EconomySystem economy;

    private GridSystem grid;
    private Vector2Int gridPosition;

    public Behaviour outline;
    private Renderer rend;

    [Header("Combat")]
    public float fireCooldown = 0f;
    public ProjectilePool projectilePool;

    private List<Enemy> enemies = new List<Enemy>();
    [System.Serializable]
    public class UpgradeLevel
    {
        public int maxHealthIncrease;
        public int damageIncrease;
        public float attackSpeedIncrease;
        public float rangeIncrease;
        public string description;
        public int sellValueIncrase;
        public int upgradeCost;
    }

    public List<UpgradeLevel> upgradeLevels = new List<UpgradeLevel>();

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        SetOutline(false);
    }

    public void SetOutline(bool value)
    {
        if (rend == null) return;

        if (rend.materials.Length < 2) return;

        var mats = rend.materials;
        mats[1].SetFloat("_Glow", value ? 2f : 0f);
        rend.materials = mats;
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (economy == null)
            economy = FindFirstObjectByType<EconomySystem>();
        
        if (projectilePool == null)
            projectilePool = FindFirstObjectByType<ProjectilePool>();

        if (progressionSystem == null)
            progressionSystem = FindAnyObjectByType<ProgressionSystem>();
        if (enemySpawner == null)
            enemySpawner = FindAnyObjectByType<EnemySpawner>();

            enemySpawner.objectsInLayerList.Add(this);
    }

    public void Upgrade()
    {
        if (economy == null) return;

        if (upgradeLevel >= 6) {
            Debug.Log("Maximum upgrade level reached!");
            return;
        }

        int upgradeCost = upgradeLevels[upgradeLevel-1].upgradeCost;

        if (economy.currentData < upgradeCost)
        {
            Debug.Log("Not enough data to upgrade!");
            return;
        }

        economy.SpendData(upgradeCost);
        upgradeLevel++;

        // Example stat increases
        attackSpeed = upgradeLevels[upgradeLevel-1].attackSpeedIncrease;
        damage = upgradeLevels[upgradeLevel-1].damageIncrease;
        range = upgradeLevels[upgradeLevel-1].rangeIncrease;
        maxHealth = upgradeLevels[upgradeLevel-1].maxHealthIncrease;
        currentHealth = maxHealth; // Heal to full on upgrade
        sellValue = upgradeLevels[upgradeLevel-1].sellValueIncrase;
        description = upgradeLevels[upgradeLevel-1].description;
    }

    public void Sell()
    {
        if (grid != null)
        {
            grid.SetOccupied(gridPosition.x, gridPosition.y, false);
        }
        
        enemySpawner.objectsInLayerList.Remove(this);

        Destroy(gameObject);
        if (economy != null)
        {
            economy.AddData(sellValue);
        }
    }

    public void Initialize(GridSystem gridSystem, Vector2Int tilePos)
    {
        grid = gridSystem;
        gridPosition = tilePos;
    }

    void OnDestroy()
    {
        if (grid != null)
        {
            grid.SetOccupied(gridPosition.x, gridPosition.y, false);
        }
    }

    Enemy FindTarget()
    {
        Enemy bestTarget = null;
        int bestProgress = -1;

        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy e in allEnemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist > range) continue;

            int progress = e.GetPathProgress();

            if (progress > bestProgress)
            {
                bestProgress = progress;
                bestTarget = e;
            }
        }

        return bestTarget;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown > 0) return;

        Enemy target = FindTarget();

        if (target == null) return;

        Shoot(target);

        fireCooldown = 1f / attackSpeed;

    }

    void Shoot(Enemy target)
    {
        Projectile proj = projectilePool.GetProjectile(projectilePrefab);

        proj.transform.position = transform.position;

        proj.Initialize(target, projectilePool, damage, projectilePrefab);
    }

}
