using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Configuration")]
    public List<WaveConfig> waves;
    public List<Transform> waypoints;

    [Header("References")]
    public EnemyPool enemyPool;

    private int currentWaveIndex = 0;
    private int activeEnemies = 0;
    private bool isWaveActive = false;

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
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
        GameEvents.OnWaveStart?.Invoke();
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

        // Wait for all enemies to be defeated
        yield return new WaitUntil(() => activeEnemies <= 0);

        EndWave();
    }

    private void SpawnEnemy(EnemyConfig config)
    {
        if (enemyPool == null)
        {
            Debug.LogError("EnemyPool not assigned!");
            return;
        }

        Enemy enemy = enemyPool.Get();
        // Assuming waypoints has at least one point
        if (waypoints != null && waypoints.Count > 0)
            enemy.transform.position = waypoints[0].position;

        enemy.Initialize(config, waypoints);
        activeEnemies++;
    }

    private void HandleEnemyKilled(Enemy enemy, int bits)
    {
        activeEnemies--;
        enemyPool.ReturnToPool(enemy);
    }

    private void EndWave()
    {
        isWaveActive = false;
        currentWaveIndex++;
        GameEvents.OnWaveEnd?.Invoke();
        Debug.Log($"Wave {currentWaveIndex} Complete");
    }
}
