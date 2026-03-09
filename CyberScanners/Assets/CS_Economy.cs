using UnityEngine;

public class EconomySystem : MonoBehaviour
{
    public int currentData = 0;

    public void AddData(int amount)
    {
        currentData += amount;
        Debug.Log("Data: " + currentData);
    }

    public bool SpendData(int amount)
    {
        if (currentData >= amount)
        {
            currentData -= amount;
            return true;
        }

        return false;
    }
}