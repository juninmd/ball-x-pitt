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
    /// Handles GameEvents for wave start and end, and interacts with the EnemyPool.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }

        [Header("Configuration")]
        [Tooltip("Ordered list of Wave Configurations to spawn.")]
        [SerializeField] private List<WaveConfig> waves;

        [Tooltip("Waypoints for enemies to follow. The first waypoint is the spawn point.")]
        [SerializeField] private List<Transform> waypoints;

        [Tooltip("If true, the first wave starts automatically after Start().")]
        [SerializeField] private bool autoStart = false;

        [Tooltip("Time to wait between waves (if not defined in WaveConfig).")]
        [SerializeField] private float timeBetweenWaves = 5f;

        [Header("Debug Info")]
        [SerializeField] private int currentWaveIndex = 0;
        [SerializeField] private bool isSpawning = false;
        [SerializeField] private int activeEnemies = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Multiple WaveManager instances found. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            GameEvents.OnEnemyKilled += OnEnemyRemoved;
            GameEvents.OnEnemyReachedGoal += OnEnemyReached;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= OnEnemyRemoved;
            GameEvents.OnEnemyReachedGoal -= OnEnemyReached;
        }

        private void Start()
        {
            if (waves == null || waves.Count == 0)
            {
                Debug.LogError("WaveManager: No waves configured.");
                return;
            }

            if (waypoints == null || waypoints.Count == 0)
            {
                Debug.LogError("WaveManager: No waypoints configured.");
                return;
            }

            if (autoStart)
            {
                StartCoroutine(StartGameRoutine());
            }
        }

        private void OnEnemyRemoved(Enemy e, int reward)
        {
            activeEnemies--;
            CheckWaveEnd();
        }

        private void OnEnemyReached(Enemy e, int damage)
        {
            activeEnemies--;
            CheckWaveEnd();
        }

        /// <summary>
        /// Checks if the current wave is complete (no active enemies and spawning finished).
        /// </summary>
        private void CheckWaveEnd()
        {
            // Ensure activeEnemies doesn't go below zero due to race conditions or logic errors
            if (activeEnemies < 0) activeEnemies = 0;

            if (!isSpawning && activeEnemies == 0)
            {
                Debug.Log($"Wave {currentWaveIndex + 1} Cleared!");
                GameEvents.OnWaveEnd?.Invoke();

                currentWaveIndex++;

                if (currentWaveIndex < waves.Count)
                {
                    StartCoroutine(NextWaveRoutine());
                }
                else
                {
                    Debug.Log("Victory! All waves cleared.");
                    // Optional: GameEvents.OnVictory?.Invoke();
                }
            }
        }

        private IEnumerator NextWaveRoutine()
        {
            Debug.Log($"Next wave starting in {timeBetweenWaves} seconds...");
            yield return new WaitForSeconds(timeBetweenWaves);
            StartNextWave();
        }

        private IEnumerator StartGameRoutine()
        {
            yield return new WaitForSeconds(2f); // minimal delay for initialization
            StartNextWave();
        }

        public void StartNextWave()
        {
            if (isSpawning)
            {
                Debug.LogWarning("WaveManager: Already spawning a wave.");
                return;
            }

            if (currentWaveIndex >= waves.Count)
            {
                Debug.Log("WaveManager: No more waves to spawn.");
                return;
            }

            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }

        private IEnumerator SpawnWave(WaveConfig config)
        {
            isSpawning = true;
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            foreach (var group in config.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    // Wait for spawn rate interval
                    yield return new WaitForSeconds(group.spawnRate);
                }
                // Wait for group interval
                if (config.timeBetweenGroups > 0)
                {
                    yield return new WaitForSeconds(config.timeBetweenGroups);
                }
            }

            isSpawning = false;
            // Check immediately in case all enemies died while spawning
            CheckWaveEnd();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.LogError("WaveManager: EnemyPool instance is null! Cannot spawn enemy.");
                return;
            }

            activeEnemies++;
            Enemy enemy = EnemyPool.Instance.Get(config.prefab);

            // Validate if Get returned null (e.g. if prefab was missing in config)
            if (enemy == null)
            {
                Debug.LogError($"WaveManager: Failed to get enemy from pool for config {config.name}");
                activeEnemies--; // Revert count
                return;
            }

            enemy.Initialize(config, waypoints);
        }
    }
}
