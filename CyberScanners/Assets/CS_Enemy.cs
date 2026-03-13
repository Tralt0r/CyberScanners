using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100;
    public float moveSpeed = 2;
    public int dataReward = 10;

    public float currentHealth;

    [Header("Pathing")]
    public EnemyPath path;
    private int currentWaypoint = 0;
    public EconomySystem economy;

    [Header("Core Damage")]
    public int damageToCore = 10;
    public CoreSystem core;

    [System.Serializable]
    public class DeathSpawn
    {
        public GameObject enemyPrefab;
        public int count = 1;
    }

    [Header("Death Spawn")]
    public List<DeathSpawn> spawnOnDeath = new List<DeathSpawn>();

    void Start()
    {
        if (economy == null)
            economy = FindFirstObjectByType<EconomySystem>();

        if (path == null)
            path = FindFirstObjectByType<EnemyPath>();
            
        if (core == null)
            core = FindFirstObjectByType<CoreSystem>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (path == null || path.WaypointCount() == 0) return;

        Transform target = path.GetWaypoint(currentWaypoint);
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypoint++;
            if (currentWaypoint >= path.WaypointCount())
            {
                ReachCore();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (economy != null)
            economy.AddData(dataReward);

        SpawnDeathEnemies();

        Destroy(gameObject);
    }

    void ReachCore()
    {
        if (core != null)
        {
            core.TakeDamage(damageToCore);
        }

        Destroy(gameObject);
    }

    public int GetPathProgress()
    {
        return currentWaypoint;
    }
    public void SetPathProgress(int waypointIndex)
    {
        currentWaypoint = waypointIndex;
    }
    void SpawnDeathEnemies()
    {
        if (spawnOnDeath == null || spawnOnDeath.Count == 0)
            return;

        foreach (var spawn in spawnOnDeath)
        {
            if (spawn.enemyPrefab == null) continue;

            for (int i = 0; i < spawn.count; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 0.3f;
                offset.y = 0;

                GameObject newEnemy = Instantiate(
                    spawn.enemyPrefab,
                    transform.position + offset,
                    Quaternion.identity
                );

                Enemy e = newEnemy.GetComponent<Enemy>();

                if (e != null)
                {
                    e.path = path;
                    e.economy = economy;
                    e.core = core;

                    // 🔥 IMPORTANT PART
                    e.SetPathProgress(currentWaypoint);
                }
            }
        }
    }
}