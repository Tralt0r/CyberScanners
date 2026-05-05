using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreSystem : MonoBehaviour
{
    [Header("Core Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject loseScreen;
    public int damageWithdrawn = 0;

    public CS_QuickOptions quickOptions;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (quickOptions.godMode == false)
        {
            Debug.Log("God Mode is ON. No damage taken!");
            currentHealth -= damage;
            return;
        }
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
            delayedMainMenu(3f); // Delay before returning to main menu
        }
    }

    IEnumerator delayedMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Load main menu scene here (e.g., using SceneManager.LoadScene)
        SceneManager.LoadScene("MainMenu");
    }
}