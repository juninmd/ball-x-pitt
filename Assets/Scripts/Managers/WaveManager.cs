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
        [SerializeField] private float timeBetweenWaves = 5f;

        private int currentWaveIndex = 0;
        private bool isSpawning = false;
        private int activeEnemies = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
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
            if (autoStart) StartCoroutine(StartGameRoutine());
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

        private void CheckWaveEnd()
        {
            if (activeEnemies < 0) activeEnemies = 0;

            if (!isSpawning && activeEnemies == 0)
            {
                CheckWaveEndAndTriggerEvent();
                UpdateWaveIndexAndScheduleNext();
            }
        }

        private void CheckWaveEndAndTriggerEvent()
        {
            GameEvents.OnWaveEnd?.Invoke();
        }

        private void UpdateWaveIndexAndScheduleNext()
        {
            currentWaveIndex++;
            if (currentWaveIndex < waves.Count)
            {
                StartCoroutine(NextWaveRoutine());
            }
        }

        private IEnumerator NextWaveRoutine()
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            StartNextWave();
        }

        private IEnumerator StartGameRoutine()
        {
            yield return new WaitForSeconds(2f);
            StartNextWave();
        }

        public void StartNextWave()
        {
            if (!isSpawning && currentWaveIndex < waves.Count)
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }

        private IEnumerator SpawnWave(WaveConfig config)
        {
            isSpawning = true;
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            yield return StartCoroutine(SpawnWaveWithGroups(config));

            isSpawning = false;
            CheckWaveEnd();
        }

        private IEnumerator SpawnWaveWithGroups(WaveConfig config)
        {
            foreach (var group in config.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(group.spawnRate);
                }
                if (config.timeBetweenGroups > 0)
                    yield return new WaitForSeconds(config.timeBetweenGroups);
            }
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            activeEnemies++;
            Enemy enemy = EnemyPool.Instance.Get(config.prefab);
            if (enemy == null)
            {
                activeEnemies--;
                return;
            }
            enemy.Initialize(config, waypoints);
        }
    }
}
