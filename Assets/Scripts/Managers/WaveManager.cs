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
        private int activeEnemies = 0;

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

        private void OnEnable()
        {
            GameEvents.OnEnemyKilled += HandleEnemyRemoved;
            GameEvents.OnEnemyReachedGoal += HandleEnemyRemoved;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= HandleEnemyRemoved;
            GameEvents.OnEnemyReachedGoal -= HandleEnemyRemoved;
        }

        private void Start()
        {
            if (autoStart)
            {
                StartCoroutine(StartGameRoutine());
            }
        }

        private void HandleEnemyRemoved(Enemy enemy, int value)
        {
            activeEnemies--;
            CheckWaveCompletion();
        }

        private void CheckWaveCompletion()
        {
            if (!isSpawning && activeEnemies <= 0)
            {
                Debug.Log($"Wave {currentWaveIndex + 1} Cleared!");
                GameEvents.OnWaveEnd?.Invoke();

                // Prepare for next wave logic
                currentWaveIndex++;
                if (currentWaveIndex < waves.Count)
                {
                    // Wait for delay defined in the previous wave config
                    float delay = waves[currentWaveIndex - 1].timeBetweenGroups;
                    StartCoroutine(WaitAndStartNextWave(delay));
                }
                else
                {
                    Debug.Log("All waves completed! Victory!");
                }
            }
        }

        private IEnumerator WaitAndStartNextWave(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartNextWave();
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
        }

        private IEnumerator StartGameRoutine()
        {
            yield return new WaitForSeconds(2f); // Initial warm-up
            StartNextWave();
        }

        private IEnumerator SpawnWave(WaveConfig waveConfig)
        {
            isSpawning = true;
            activeEnemies = 0;

            Debug.Log($"Starting Wave {currentWaveIndex + 1}");
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            foreach (var group in waveConfig.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    activeEnemies++;
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(group.spawnRate);
                }
            }

            isSpawning = false;

            // Check immediately in case all enemies died during spawn
            CheckWaveCompletion();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.LogError("EnemyPool is missing from the scene!");
                return;
            }

            // Get enemy from pool
            Enemy enemy = EnemyPool.Instance.Get();

            if (enemy != null)
            {
                enemy.Initialize(config, waypoints);
            }
        }
    }
}
