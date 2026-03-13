using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    private Dictionary<Projectile, Queue<Projectile>> pools = new Dictionary<Projectile, Queue<Projectile>>();

    public Projectile GetProjectile(Projectile prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<Projectile>();
        }

        Queue<Projectile> pool = pools[prefab];

        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }

        Projectile p = Instantiate(prefab, transform);
        p.gameObject.SetActive(false);
        return p;
    }

    public void ReturnProjectile(Projectile proj, Projectile prefab)
    {
        proj.gameObject.SetActive(false);

        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<Projectile>();

        pools[prefab].Enqueue(proj);
    }
}