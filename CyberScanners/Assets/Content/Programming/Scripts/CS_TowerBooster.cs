using System.Collections;
using UnityEngine;

public class CS_TowerBooster : MonoBehaviour
{
    public int level = 1;

    public Tower tower; // the booster tower itself

    [Header("Boost Values")]
    public float damageBoostPercent;
    public float attackSpeedBoostPercent;
    public float rangeBoostPercent;
    public bool attacksAllEnemies = false;

    public float checkInterval = 1f;

    public float damageMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public float rangeMultiplier = 1f;

    public float boosterRange = 1;

    void Start()
    {
        StartCoroutine(BoostLoop());
    }

    void Update()
    {
        if (tower != null)
            level = tower.upgradeLevel;

        UpdateBoostValues();
    }

    void UpdateBoostValues()
    {
        switch (level)
        {
            case 1:
                damageBoostPercent = 1f;
                attackSpeedBoostPercent = 1f;
                rangeBoostPercent = 1f;
                boosterRange = 1;
                break;

            case 2:
                damageBoostPercent = 2f;
                attackSpeedBoostPercent = 1.5f;
                rangeBoostPercent = 1.5f;
                boosterRange = 1.2f;
                break;

            case 3:
                damageBoostPercent = 3f;
                attackSpeedBoostPercent = 1.8f;
                rangeBoostPercent = 1.8f;
                boosterRange = 1.4f;
                break;

            case 4:
                damageBoostPercent = 5f;
                attackSpeedBoostPercent = 2f;
                rangeBoostPercent = 2f;
                boosterRange = 1.6f;
                attacksAllEnemies = true;
                break;

            case 5:
                damageBoostPercent = 7f;
                attackSpeedBoostPercent = 2f;
                rangeBoostPercent = 3f;
                boosterRange = 1.8f;
                break;

            case 6:
                damageBoostPercent = 10f;
                attackSpeedBoostPercent = 3f;
                rangeBoostPercent = 4f;
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
            Tower t = hit.GetComponent<Tower>();

            if (t == null) continue;
            if (t == tower) continue;

            t.damage = Mathf.RoundToInt(t.upgradeLevels[t.upgradeLevel-1].damageIncrease * damageBoostPercent);
            t.attackSpeed = t.upgradeLevels[t.upgradeLevel-1].attackSpeedIncrease * attackSpeedMultiplier;
            t.range =  t.upgradeLevels[t.upgradeLevel-1].rangeIncrease * rangeBoostPercent;

            if (attacksAllEnemies)
            {
                if (!t.attackableEnemyTypes.Contains(1))
                    t.attackableEnemyTypes.Add(1);

                if (!t.attackableEnemyTypes.Contains(2))
                    t.attackableEnemyTypes.Add(2);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (tower == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, boosterRange);
    }
}