using UnityEngine;

public class CoreSystem : MonoBehaviour
{
    [Header("Core Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject loseScreen;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"Core took {damage} damage! Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Core destroyed! Game Over!");
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    void Update()
    {
        if (currentHealth <= 0 && loseScreen != null)
        {
            loseScreen.SetActive(true);
        }
    }
}