using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Managers
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<WaveConfig> waves;
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private float timeBetweenWaves = 5f;
        [SerializeField] private bool autoStart = false;

        private int currentWaveIndex = 0;
        private int activeEnemies = 0;
        private bool isSpawning = false;

        private void OnEnable()
        {
            GameEvents.OnEnemyKilled += HandleEnemyDefeated;
            GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= HandleEnemyDefeated;
            GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
        }

        private void Start()
        {
            if (autoStart && waves != null && waves.Count > 0)
            {
                StartCoroutine(StartWaveCoroutine());
            }
        }

        private IEnumerator StartWaveCoroutine()
        {
            if (currentWaveIndex >= waves.Count) yield break;

            GameEvents.OnWaveStart?.Invoke(currentWaveIndex);
            WaveConfig currentWave = waves[currentWaveIndex];
            isSpawning = true;

            foreach (var group in currentWave.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    float waitTime = group.spawnRate > 0f ? 1f / group.spawnRate : 0f;
                    if (waitTime > 0f)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                    else
                    {
                        yield return null;
                    }
                }
                yield return new WaitForSeconds(currentWave.timeBetweenGroups);
            }

            isSpawning = false;
            CheckWaveEndAndTriggerEvent();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null || config.prefab == null) return;

            Enemies.Enemy enemy = EnemyPool.Instance.Get(config.prefab);
            enemy.Initialize(config, waypoints, config.prefab);
            activeEnemies++;
        }

        private void HandleEnemyDefeated(Enemies.Enemy enemy)
        {
            activeEnemies--;
            CheckWaveEndAndTriggerEvent();
        }

        private void HandleEnemyReachedGoal(Enemies.Enemy enemy, int damage)
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
