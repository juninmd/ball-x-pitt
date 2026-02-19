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
    /// Implements the Wave Spawning Logic requirement.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }

        [Header("Configuration")]
        [Tooltip("List of Wave Configurations to spawn in order.")]
        [SerializeField] private List<WaveConfig> waves;

        [Tooltip("Waypoints for enemies to follow. First waypoint is spawn point.")]
        [SerializeField] private List<Transform> waypoints;

        [Tooltip("Automatically start the first wave on play.")]
        [SerializeField] private bool autoStart = false;

        [Tooltip("Time to wait between waves (if not defined in WaveConfig).")]
        [SerializeField] private float timeBetweenWaves = 5f;

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
            GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= HandleEnemyRemoved;
            GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
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

        private void HandleEnemyReachedGoal(Enemy enemy, int damage)
        {
            activeEnemies--;
            CheckWaveCompletion();
        }

        /// <summary>
        /// Checks if the current wave is complete (no active enemies and spawning finished).
        /// Triggers OnWaveEnd if true.
        /// </summary>
        private void CheckWaveCompletion()
        {
            // Only end wave if spawning is finished and no enemies remain
            if (!isSpawning && activeEnemies <= 0)
            {
                // Ensure count doesn't go negative due to race conditions
                activeEnemies = 0;

                Debug.Log($"Wave {currentWaveIndex + 1} Cleared!");
                GameEvents.OnWaveEnd?.Invoke();

                // Prepare for next wave logic
                currentWaveIndex++;
                if (currentWaveIndex < waves.Count)
                {
                    StartCoroutine(WaitAndStartNextWave(timeBetweenWaves));
                }
                else
                {
                    Debug.Log("All waves completed! Victory!");
                    // Trigger generic victory event or UI here
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
            else
            {
                Debug.Log("No more waves to spawn.");
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

            // Calculate total enemies for UI or logic if needed
            int totalEnemiesInWave = 0;
            if (waveConfig.enemyGroups != null)
            {
                foreach (var group in waveConfig.enemyGroups) totalEnemiesInWave += group.count;
            }

            Debug.Log($"Starting Wave {currentWaveIndex + 1} with {totalEnemiesInWave} enemies.");
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            if (waveConfig.enemyGroups != null)
            {
                for (int i = 0; i < waveConfig.enemyGroups.Count; i++)
                {
                    var group = waveConfig.enemyGroups[i];
                    for (int j = 0; j < group.count; j++)
                    {
                        activeEnemies++;
                        SpawnEnemy(group.enemyConfig);
                        yield return new WaitForSeconds(group.spawnRate);
                    }

                    // Wait between groups (if there are more groups)
                    if (i < waveConfig.enemyGroups.Count - 1)
                    {
                        yield return new WaitForSeconds(waveConfig.timeBetweenGroups);
                    }
                }
            }

            isSpawning = false;

            // Check immediately in case all enemies died during spawn (unlikely but possible)
            CheckWaveCompletion();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.LogError("EnemyPool is missing from the scene!");
                activeEnemies--;
                return;
            }

            // Get enemy from pool
            Enemy enemy = EnemyPool.Instance.Get(config.prefab);

            if (enemy != null)
            {
                enemy.Initialize(config, waypoints);
            }
            else
            {
                activeEnemies--;
            }
        }
    }
}
