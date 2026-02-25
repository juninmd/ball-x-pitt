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

        [Tooltip("Waypoints for enemies to follow. First waypoint is spawn point.")]
        [SerializeField] private List<Transform> waypoints;

        [Tooltip("Automatically start the first wave on play.")]
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
            if (autoStart)
            {
                StartCoroutine(StartGame());
            }
        }

        private void OnEnemyRemoved(Enemy e, int v)
        {
            activeEnemies--;
            CheckWaveEnd();
        }

        private void OnEnemyReached(Enemy e, int d)
        {
            activeEnemies--;
            CheckWaveEnd();
        }

        private void CheckWaveEnd()
        {
            if (!isSpawning && activeEnemies <= 0)
            {
                activeEnemies = 0;
                GameEvents.OnWaveEnd?.Invoke();
                currentWaveIndex++;

                if (currentWaveIndex < waves.Count)
                {
                    StartCoroutine(NextWaveRoutine());
                }
                else
                {
                    Debug.Log("Victory! All waves cleared.");
                }
            }
        }

        private IEnumerator NextWaveRoutine()
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            StartNextWave();
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(2f);
            StartNextWave();
        }

        public void StartNextWave()
        {
            if (isSpawning || currentWaveIndex >= waves.Count) return;
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }

        private IEnumerator SpawnWave(WaveConfig config)
        {
            isSpawning = true;
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            foreach (var group in config.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(group.spawnRate);
                }
                yield return new WaitForSeconds(config.timeBetweenGroups);
            }

            isSpawning = false;
            CheckWaveEnd();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.LogError("EnemyPool instance is null!");
                return;
            }

            activeEnemies++;
            Enemy enemy = EnemyPool.Instance.Get(config.prefab);
            enemy.Initialize(config, waypoints);
        }
    }
}
