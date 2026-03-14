using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSystem : MonoBehaviour
{
    [Header("Wave Info")]
    public int currentWave = 1;

    [Header("Enemy Scaling")]
    public int baseEnemyCount = 5;
    public float enemyHealthMultiplier = 1.15f;
    public float enemySpeedMultiplier = 1.02f;

    [Header("Rewards")]
    public int baseWaveReward = 50;

    [Header("References")]
    public EnemySpawner spawner;
    public EconomySystem economy;
    public CoreSystem core;
    public CS_WaveEnemyTable enemyTable;
    private List<Enemy> activeEnemies = new List<Enemy>();

    void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        int enemyCount = Mathf.RoundToInt(baseEnemyCount * currentWave);

        float healthScale = Mathf.Pow(enemyHealthMultiplier, currentWave);
        float speedScale = Mathf.Pow(enemySpeedMultiplier, currentWave);

        StartCoroutine(SpawnAndTrackWave(enemyCount, healthScale, speedScale));
    }

    private IEnumerator SpawnAndTrackWave(int count, float healthScale, float speedScale)
    {
        activeEnemies.Clear();

        // Get enemies for this wave from the table
        List<GameObject> enemies = enemyTable.GetEnemiesForWave(currentWave);

        foreach (var prefab in enemies)
        {
            GameObject enemyGO = spawner.SpawnEnemy(prefab, healthScale, speedScale);

            Enemy e = enemyGO.GetComponent<Enemy>();
            if (e != null)
                activeEnemies.Add(e);

            yield return new WaitForSeconds(spawner.spawnDelay);
        }

        // Wait until all enemies are dead OR core is destroyed
        while (activeEnemies.Count > 0 && core.IsAlive())
        {
            activeEnemies.RemoveAll(e => e == null);
            yield return null;
        }

        EndWave();
    }

    public void EndWave()
    {
        int reward = baseWaveReward * currentWave;
        economy.AddData(reward);

        Debug.Log($"Wave {currentWave} Completed. Reward: {reward}");

        currentWave++;

        if (core.IsAlive())
        {
            StartNextWave();
        }
    }
}
