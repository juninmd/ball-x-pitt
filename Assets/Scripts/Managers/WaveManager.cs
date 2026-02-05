using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.ScriptableObjects;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    /// <summary>
    /// Manages the spawning of enemy waves.
    /// </summary>
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

            // Wait before starting next wave automatically
            if (waveConfig.timeBetweenGroups > 0 && currentWaveIndex < waves.Count)
            {
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

            Enemy enemy = EnemyPool.Instance.Get();
            if (enemy != null)
            {
                enemy.Initialize(config, waypoints);
            }
        }
    }
}
