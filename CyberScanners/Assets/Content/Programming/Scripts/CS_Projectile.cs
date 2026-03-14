using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Projectile prefabType;
    public float speed = 25f;
    public int damage = 10;

    [Header("AOE Settings")]
    public float explosionRadius; // 0 = single target
    Vector3 lastExplosionPos;
    float explosionGizmoTimer = 0f;
    private Enemy target;
    private ProjectilePool pool;

    public void Initialize(Enemy enemy, ProjectilePool projectilePool, int dmg, Projectile prefab)
    {
        target = enemy;
        pool = projectilePool;
        damage = dmg;
        prefabType = prefab;

        gameObject.SetActive(true);

        if (target != null)
        {
            Vector3 lookDir = (target.transform.position - transform.position).normalized;
            if (lookDir != Vector3.zero)
                transform.forward = lookDir;
        }
    }

    void Update()
    {
        if (target == null)
        {
            pool.ReturnProjectile(this, prefabType);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            Explode();
        }
        if (explosionGizmoTimer > 0)
        {
            explosionGizmoTimer -= Time.deltaTime;
        }
    }

    void Explode()
    {
        lastExplosionPos = target.transform.position;
        explosionGizmoTimer = 1f; // show gizmo briefly

        if (explosionRadius <= 0f)
        {
            target.TakeDamage(damage);
        }
        else
        {
            Collider[] hits = Physics.OverlapSphere(lastExplosionPos, explosionRadius);

            foreach (Collider hit in hits)
            {
                Enemy e = hit.GetComponent<Enemy>();
                if (e != null)
                {
                    e.TakeDamage(damage);
                }
            }
        }

        StartCoroutine(ReturnAfterGizmo());
    }

    System.Collections.IEnumerator ReturnAfterGizmo()
    {
        yield return new WaitForSeconds(0.2f);
        pool.ReturnProjectile(this, prefabType);
    }
    
    void OnDrawGizmos()
    {
        if (explosionGizmoTimer > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastExplosionPos, explosionRadius);
        }
    }
}