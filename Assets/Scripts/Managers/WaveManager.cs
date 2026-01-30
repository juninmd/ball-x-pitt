using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Configuration")]
    public List<WaveConfig> waves;
    public Transform spawnPoint;

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;

    private void Start()
    {
        // Waiting for UI or GameManager to call StartWave
    }

    public void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves complete!");
            return;
        }

        if (isWaveActive) return;

        StartCoroutine(SpawnWaveRoutine(waves[currentWaveIndex]));
    }

    private IEnumerator SpawnWaveRoutine(WaveConfig waveConfig)
    {
        isWaveActive = true;
        if (GameEvents.OnWaveStart != null) GameEvents.OnWaveStart.Invoke();
        Debug.Log($"Starting Wave {currentWaveIndex + 1}");

        foreach (var group in waveConfig.enemyGroups)
        {
            for (int i = 0; i < group.count; i++)
            {
                SpawnEnemy(group.enemyConfig);
                yield return new WaitForSeconds(group.spawnRate);
            }
            yield return new WaitForSeconds(waveConfig.timeBetweenGroups);
        }

        currentWaveIndex++;
        isWaveActive = false;

        // Note: Real wave end logic should wait for enemies to die.
        // Simplified here to just mark spawning end.
        if (GameEvents.OnWaveEnd != null) GameEvents.OnWaveEnd.Invoke();
        Debug.Log($"Wave {currentWaveIndex} Spawning Complete");
    }

    private void SpawnEnemy(EnemyConfig config)
    {
        if (config.prefab == null)
        {
             Debug.LogWarning($"Enemy config {config.enemyName} has no prefab!");
             return;
        }

        // Logic to instantiate enemy from pool would go here
        // var enemy = EnemyPool.Instance.Get(); ...

        Debug.Log($"Spawning Enemy: {config.enemyName} at {spawnPoint?.position}");
    }
}
