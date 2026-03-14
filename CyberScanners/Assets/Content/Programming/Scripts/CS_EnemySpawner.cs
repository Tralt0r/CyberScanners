using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    [Header("References")]
    public EnemyPath path;
    public EconomySystem economy;
    public CoreSystem core;
    public ProgressionSystem progressionSystem;

    [Header("Spawn Settings")]
    public float spawnDelay = 0.5f;

    public string targetLayerName = "YourLayerName";
    public List<Tower> objectsInLayerList = new List<Tower>();

    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject prefab;
        public int cost = 1;
        public int minWave = 1;
    }

    [Header("Enemy Types")]
    public List<EnemyEntry> enemyTypes = new List<EnemyEntry>();

    [Header("Wave Budget")]
    public int baseEnemyBudget = 10;
    public int budgetIncreasePerWave = 5;



    void Start()
    {
        if (path == null)
            path = FindFirstObjectByType<EnemyPath>();

        if (economy == null)
            economy = FindFirstObjectByType<EconomySystem>();

        if (spawnPoint == null)
            spawnPoint = this.transform;

        int layerId = LayerMask.NameToLayer(targetLayerName);

        GameObject[] allObjectsInScene = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        objectsInLayerList = allObjectsInScene
            .Where(obj => obj.layer == layerId)
            .Select(obj => obj.GetComponent<Tower>())
            .Where(t => t != null)
            .ToList();
    }

    public void SpawnWave(int wave, float healthScale, float speedScale)
    {
        int budget = baseEnemyBudget + (wave * budgetIncreasePerWave);

        StartCoroutine(SpawnWaveCoroutine(budget, wave, healthScale, speedScale));

        if (objectsInLayerList != null)
        {
            foreach (Tower tower in objectsInLayerList)
            {
                if (tower.ignoreDamageNerf)
                {
                    tower.damage = progressionSystem.currentWave * (int)progressionSystem.enemyHealthMultiplier * tower.damage;
                }
            }
        }
    }

    IEnumerator SpawnWaveCoroutine(int budget, int wave, float healthScale, float speedScale)
    {
        while (budget > 0)
        {
            EnemyEntry entry = ChooseEnemy(wave, budget);

            if (entry == null)
                yield break;

            SpawnEnemy(entry.prefab, healthScale, speedScale);

            budget -= entry.cost;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public GameObject SpawnSingleEnemy(float healthScale, float speedScale)
    {
        GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy e = enemyGO.GetComponent<Enemy>();

        if (e != null)
        {
            e.maxHealth *= healthScale;
            e.moveSpeed *= speedScale;

            e.path = path;
            e.economy = economy;
            e.core = core;
        }

        return enemyGO;
    }

    public GameObject SpawnEnemy(GameObject prefab, float healthScale, float speedScale)
    {
        GameObject enemyGO = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Enemy e = enemyGO.GetComponent<Enemy>();

        if (e != null)
        {
            e.maxHealth *= healthScale;
            e.moveSpeed *= speedScale;

            e.path = path;
            e.economy = economy;
            e.core = core;
        }

        return enemyGO;
    }

    EnemyEntry ChooseEnemy(int wave, int budget)
    {
        List<EnemyEntry> valid = enemyTypes
            .Where(e => e.minWave <= wave && e.cost <= budget)
            .ToList();

        if (valid.Count == 0)
            return null;

        // prefer stronger enemies
        valid.Sort((a, b) => b.cost.CompareTo(a.cost));

        // 70% chance strongest enemy
        if (Random.value < 0.7f)
            return valid[0];

        // otherwise random weaker enemy
        return valid[Random.Range(0, valid.Count)];
    }
}