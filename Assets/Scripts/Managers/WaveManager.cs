using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Configuration")]
    [SerializeField] private List<WaveConfig> waves;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float timeBetweenWaves = 5f;

    [Header("References")]
    [SerializeField] private EnemyPool enemyPool;

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

    private void Start()
    {
        if (enemyPool == null)
        {
            Debug.LogError("WaveManager: EnemyPool reference is missing!");
            enabled = false;
            return;
        }

        // Optional: Auto-start for testing
        // StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(2f);
        StartWave();
    }

    public void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves complete! Victory!");
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

        // Wait until all enemies are cleared
        yield return new WaitUntil(() => activeEnemies <= 0);

        EndWave();
    }

    private void SpawnEnemy(EnemyConfig config)
    {
        Enemy enemy = enemyPool.Get();
        enemy.transform.position = waypoints[0].position; // Ensure start position
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

        if (currentWaveIndex < waves.Count)
        {
            StartCoroutine(NextWaveCooldown());
        }
    }

    private IEnumerator NextWaveCooldown()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartWave();
    }
}
