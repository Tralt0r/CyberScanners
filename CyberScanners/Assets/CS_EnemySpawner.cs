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

    public string targetLayerName = "YourLayerName"; // Set this in the Inspector
    public List<Tower> objectsInLayerList = new List<Tower>();

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

    public void SpawnWave(int count, float healthScale, float speedScale)
    {
        // Start a coroutine to spawn enemies with delay
        StartCoroutine(SpawnWaveCoroutine(count, healthScale, speedScale));
        
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

    private IEnumerator SpawnWaveCoroutine(int count, float healthScale, float speedScale)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Enemy e = enemy.GetComponent<Enemy>();

            if (e == null)
            {
                Debug.LogError("Enemy prefab is missing the Enemy component!");
            }
            else
            {
                e.maxHealth *= healthScale;
                e.moveSpeed *= speedScale;

                e.path = path;
                e.economy = economy;
                e.core = core;
            }

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
}