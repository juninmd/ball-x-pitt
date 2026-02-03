using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.ScriptableObjects;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }

        [Header("Configuration")]
        [SerializeField] private List<WaveConfig> waves;
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private bool autoStart = false;

        private int currentWaveIndex = 0;
        private bool isSpawning = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (autoStart)
            {
                StartCoroutine(StartGameRoutine());
            }
        }

        public void StartNextWave()
        {
            if (!isSpawning && currentWaveIndex < waves.Count)
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            }
        }

        private IEnumerator StartGameRoutine()
        {
            yield return new WaitForSeconds(2f); // Initial warm-up
            StartNextWave();
        }

        private IEnumerator SpawnWave(WaveConfig waveConfig)
        {
            isSpawning = true;
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            foreach (var group in waveConfig.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(group.spawnRate);
                }

                // Optional: Wait between groups? Currently logic is sequential.
            }

            isSpawning = false;
            currentWaveIndex++;

            if (currentWaveIndex >= waves.Count)
            {
                Debug.Log("All waves spawned!");
                // GameEvents.OnGameWin?.Invoke(); // If we had such event
            }
            else
            {
                // Auto start next wave after delay defined in config
                if (waveConfig.timeBetweenGroups > 0)
                {
                    yield return new WaitForSeconds(waveConfig.timeBetweenGroups);
                    StartNextWave();
                }
            }
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.LogError("EnemyPool missing in scene!");
                return;
            }

            // We need to ensure the pool has the correct prefab if we want to support multiple types.
            // However, generic ObjectPool<T> usually has ONE prefab.
            // For a complex TD, we'd need a PoolManager or Dictionary<string, Pool>.
            // Given the scope/time, we'll assume the Pool is generic or the EnemyConfig prefab matches the pool's prefab.
            // OR: We instantiate directly if pool doesn't match?
            // Better: The EnemyPool handles generic 'Enemy' type. The 'Enemy' script then initializes visually/stat-wise.
            // So we Get() a generic Enemy shell, and Initialize() it with the config (which sets stats).
            // Visuals: If models differ significantly, we'd need multiple pools.
            // For this implementation, we assume the Enemy Prefab in the pool is a container that adapts,
            // OR we accept that for this demo, only one visual type is pooled.

            Enemy enemy = EnemyPool.Instance.Get();
            enemy.Initialize(config, waypoints);
        }
    }
}
