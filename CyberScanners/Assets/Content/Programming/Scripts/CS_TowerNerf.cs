using System.Collections;
using UnityEngine;

public class CS_TowerNerf : MonoBehaviour
{
    public int level = 1;

    public Tower tower; // the booster tower itself

    [Header("Nerf Values")]
    public float checkInterval = 1f;

    public float speedNerf = 0.9f;
    public float healthNerf = 0.9f;
    public float explosionNerf = 0.9f;

    public float boosterRange = 1;

    void Start()
    {
        StartCoroutine(BoostLoop());
    }

    void Update()
    {
        if (tower != null)
            level = tower.upgradeLevel;

        UpdateNerfValues();
    }

    void UpdateNerfValues()
    {
        switch (level)
        {
            case 1:
                speedNerf = 0.9f;
                healthNerf = 0.9f;
                explosionNerf = 0.9f;

                boosterRange = 1;
                break;

            case 2:
                speedNerf = 0.75f;
                healthNerf = 0.75f;
                explosionNerf = 0.75f;

                boosterRange = 1.2f;
                break;

            case 3:
                speedNerf = 0.6f;
                healthNerf = 0.6f;
                explosionNerf = 0.6f;

                boosterRange = 1.4f;
                break;

            case 4:
                speedNerf = 0.4f;
                healthNerf = 0.4f;
                explosionNerf = 0.4f;
                
                boosterRange = 1.6f;
                break;

            case 5:
                speedNerf = 0.25f;
                healthNerf = 0.25f;
                explosionNerf = 0.25f;
                
                boosterRange = 1.8f;
                break;

            case 6:
                speedNerf = 0.1f;
                healthNerf = 0.1f;
                explosionNerf = 0.1f;
                
                boosterRange = 2f;
                break;
        }
    }

    IEnumerator BoostLoop()
    {
        while (true)
        {
            ApplyBoosts();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void ApplyBoosts()
    {
        if (tower == null) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, boosterRange);

        foreach (Collider hit in hits)
        {
            Enemy e = hit.GetComponentInParent<Enemy>();

            if (e == null) continue;

            e.moveSpeed *= speedNerf;
            e.currentHealth *= healthNerf;
            e.explosionDamage *= (int)explosionNerf;
        }
    }

    void OnDrawGizmos()
    {
        if (tower == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, boosterRange);
    }
}