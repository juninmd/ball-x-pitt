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
        [SerializeField] private List<WaveConfig> waves;
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float timeBetweenWaves = 5f;
        [SerializeField] private bool autoStart = false;

        private int currentWaveIndex = 0;
        private int activeEnemies = 0;
        private bool isSpawning = false;

        private void OnEnable()
        {
            GameEvents.OnEnemyKilled += HandleEnemyKilled;
            GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= HandleEnemyKilled;
            GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
        }

        private void Start()
        {
            if (autoStart && waves.Count > 0)
            {
                StartCoroutine(StartWaveCoroutine());
            }
        }

        private IEnumerator StartWaveCoroutine()
        {
            if (currentWaveIndex >= waves.Count) yield break;

            GameEvents.OnWaveStart?.Invoke(currentWaveIndex);
            isSpawning = true;

            WaveConfig currentWave = waves[currentWaveIndex];

            foreach (EnemyGroup group in currentWave.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);

                    if (group.spawnRate <= 0f)
                    {
                        yield return null; // Prevents division by zero or infinite loops
                    }
                    else
                    {
                        yield return new WaitForSeconds(group.spawnRate);
                    }
                }

                yield return new WaitForSeconds(currentWave.timeBetweenGroups);
            }

            isSpawning = false;
            CheckWaveEndAndTriggerEvent();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            Enemy newEnemy = EnemyPool.Instance.Get(config.prefab);
            newEnemy.transform.position = spawnPoint.position;
            newEnemy.transform.rotation = spawnPoint.rotation;

            newEnemy.Initialize(config, waypoints);

            activeEnemies++;
        }

        private void HandleEnemyKilled(Enemy enemy)
        {
            activeEnemies--;
            CheckWaveEndAndTriggerEvent();
        }

        private void HandleEnemyReachedGoal(Enemy enemy, int damage)
        {
            activeEnemies--;
            CheckWaveEndAndTriggerEvent();
        }

        private void CheckWaveEndAndTriggerEvent()
        {
            if (!isSpawning && activeEnemies <= 0)
            {
                GameEvents.OnWaveEnd?.Invoke(currentWaveIndex);
                UpdateWaveIndexAndScheduleNext();
            }
        }

        private void UpdateWaveIndexAndScheduleNext()
        {
            currentWaveIndex++;
            if (currentWaveIndex < waves.Count)
            {
                Invoke(nameof(StartNextWave), timeBetweenWaves);
            }
        }

        private void StartNextWave()
        {
            StartCoroutine(StartWaveCoroutine());
        }
    }
}