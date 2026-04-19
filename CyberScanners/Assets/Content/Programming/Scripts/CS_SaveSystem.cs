using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public ProgressionSystem progressionSystem;
    public EconomySystem economySystem;

    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "SaveData.json");

    [System.Serializable]
    private class SaveData
    {
        public int currentWave;
        public int currentData;
    }

    void OnEnable()
    {
        EconomySystem.OnDataChanged += HandleDataChanged;
    }

    void OnDisable()
    {
        EconomySystem.OnDataChanged -= HandleDataChanged;
    }

    private void HandleDataChanged(int newAmount)
    {
        Save();
    }

    void Start()
    {
        if (progressionSystem == null)
            progressionSystem = FindFirstObjectByType<ProgressionSystem>();
        if (economySystem == null)
            economySystem = FindFirstObjectByType<EconomySystem>();

        Load();
    }

    public void Save()
    {
        SaveData data = new SaveData
        {
            currentWave = progressionSystem.currentWave,
            currentData = economySystem.currentData
        };

        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(SavePath, json);
        Debug.Log("[JSON] Game saved to: " + SavePath);
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[JSON] No save file found — starting fresh.");
            return;
        }

        try
        {
            string json    = File.ReadAllText(SavePath);
            SaveData data  = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
                throw new System.Exception("Save file parsed to null — file may be empty or malformed.");

            progressionSystem.currentWave = data.currentWave;
            economySystem.currentData     = data.currentData;

            Debug.Log($"[JSON] Save loaded — Wave: {data.currentWave}, Data: {data.currentData}");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[JSON] Failed to load save file, starting fresh. Reason: {e.Message}");

            File.Delete(SavePath);
        }
    }
}