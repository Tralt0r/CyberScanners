using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100;
    public float moveSpeed = 2;
    public int dataReward = 10;

    private float currentHealth;

    [Header("Pathing")]
    public EnemyPath path;
    private int currentWaypoint = 0;
    public EconomySystem economy;

    [Header("Core Damage")]
    public int damageToCore = 10;
    public CoreSystem core;

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
}