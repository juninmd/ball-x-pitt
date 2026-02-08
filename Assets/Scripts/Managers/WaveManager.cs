// NeonDefense Core System - WaveManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.ScriptableObjects;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    /// <summary>
    /// Manages the spawning of enemy waves based on WaveConfig ScriptableObjects.
    /// Handles GameEvents for wave start and end.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }

        [Header("Configuration")]
        [Tooltip("List of Wave Configurations to spawn in order.")]
        [SerializeField] private List<WaveConfig> waves;
        [Tooltip("Waypoints for enemies to follow.")]
        [SerializeField] private List<Transform> waypoints;
        [Tooltip("Automatically start the first wave on play.")]
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

        /// <summary>
        /// Starts the next wave if one is available and not currently spawning.
        /// </summary>
        public void StartNextWave()
        {
            if (isSpawning) return;

            if (currentWaveIndex < waves.Count)
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            }
            else
            {
                Debug.Log("All waves completed!");
                GameEvents.OnWaveEnd?.Invoke();
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
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");

            // Notify UI and other systems
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            foreach (var group in waveConfig.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(group.spawnRate);
                }
            }

            isSpawning = false;
            currentWaveIndex++;

            // Optional: Auto-start next wave after delay, or wait for player input/events
            if (waveConfig.timeBetweenGroups > 0 && currentWaveIndex < waves.Count)
            {
                // In a real game, we might wait for all enemies to die instead of just time
                // For now, we wait a duration or let the player trigger it.
                // Here, we wait for the duration defined in config before enabling the button again or auto-starting.
                yield return new WaitForSeconds(waveConfig.timeBetweenGroups);
                StartNextWave();
            }
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.LogError("EnemyPool is missing from the scene!");
                return;
            }

            // Note: This assumes the pool handles the instantiation or reuse.
            // In a more complex system, we'd pass the config.prefab to a PoolManager.
            // Here we use the singleton pool which recycles a generic Enemy prefab.
            Enemy enemy = EnemyPool.Instance.Get();

            if (enemy != null)
            {
                enemy.Initialize(config, waypoints);
            }
        }
    }
}
