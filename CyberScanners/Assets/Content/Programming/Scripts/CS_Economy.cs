using UnityEngine;
using System;

public class EconomySystem : MonoBehaviour
{
    public int currentData = 0;

    public static event Action<int> OnDataChanged;

    public static event Action OnInsufficientFunds;

    public void AddData(int amount)
    {
        currentData += amount;
        OnDataChanged?.Invoke(currentData);
        Debug.Log("Data: " + currentData);
    }

    public bool SpendData(int amount)
    {
        if (currentData >= amount)
        {
            currentData -= amount;
            OnDataChanged?.Invoke(currentData);
            return true;
        }

        OnInsufficientFunds?.Invoke();
        return false;
    }
}