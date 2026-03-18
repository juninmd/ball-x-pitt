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
        [Header("Configuration")]
        public List<WaveConfig> waves;
        public float timeBetweenWaves = 5f;
        public bool autoStart = false;
        public List<Transform> waypoints;

        [Header("State")]
        private int _currentWaveIndex = 0;
        private int _activeEnemies = 0;
        private bool _isSpawning = false;

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
            if (autoStart)
            {
                StartNextWave();
            }
        }

        public void StartNextWave()
        {
            if (_currentWaveIndex >= waves.Count || _isSpawning) return;

            GameEvents.OnWaveStart?.Invoke();
            StartCoroutine(SpawnWave(waves[_currentWaveIndex]));
        }

        private IEnumerator SpawnWave(WaveConfig config)
        {
            _isSpawning = true;

            foreach (var group in config.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(1f / group.spawnRate);
                }
                yield return new WaitForSeconds(config.timeBetweenGroups);
            }

            _isSpawning = false;
            CheckWaveEndAndTriggerEvent();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            Vector3 startPos = waypoints.Count > 0 ? waypoints[0].position : Vector3.zero;
            Enemy enemy = EnemyPool.Instance.Get(config.prefab, startPos, Quaternion.identity);

            // Assume the enemy script manages itself once initialized
            // enemy.Initialize(config, waypoints);

            _activeEnemies++;
        }

        private void HandleEnemyKilled(Enemy enemy)
        {
            _activeEnemies--;
            CheckWaveEndAndTriggerEvent();
        }

        private void HandleEnemyReachedGoal(Enemy enemy, int damage)
        {
            _activeEnemies--;
            CheckWaveEndAndTriggerEvent();
        }

        private void CheckWaveEndAndTriggerEvent()
        {
            if (_activeEnemies <= 0 && !_isSpawning)
            {
                GameEvents.OnWaveEnd?.Invoke(_currentWaveIndex);
                UpdateWaveIndexAndScheduleNext();
            }
        }

        private void UpdateWaveIndexAndScheduleNext()
        {
            _currentWaveIndex++;
            if (_currentWaveIndex < waves.Count)
            {
                Invoke(nameof(StartNextWave), timeBetweenWaves);
            }
            else
            {
                Debug.Log("Todas as ondas finalizadas!");
            }
        }
    }
}
