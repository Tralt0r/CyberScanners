using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemyEntry
{
    public GameObject enemyPrefab;

    [Header("Spawn Rules")]
    public int startWave = 1;
    public int spawnEveryXRounds = 1;

    [Header("Spawn Count")]
    public int baseSpawnCount = 1;
    public float spawnMultiplierPerWave = 1.1f;
}

public class CS_WaveEnemyTable : MonoBehaviour
{
    public List<WaveEnemyEntry> enemyEntries = new List<WaveEnemyEntry>();

    public List<GameObject> GetEnemiesForWave(int wave)
    {
        List<GameObject> enemiesToSpawn = new List<GameObject>();

        foreach (var entry in enemyEntries)
        {
            if (wave < entry.startWave) continue;

            int offset = wave - entry.startWave;

            if (offset % entry.spawnEveryXRounds != 0) continue;

            int spawnCount = Mathf.RoundToInt(
                entry.baseSpawnCount *
                Mathf.Pow(entry.spawnMultiplierPerWave, offset)
            );

            for (int i = 0; i < spawnCount; i++)
                enemiesToSpawn.Add(entry.enemyPrefab);
        }

        Shuffle(enemiesToSpawn);

        return enemiesToSpawn;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}