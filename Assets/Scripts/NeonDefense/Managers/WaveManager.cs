using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeonDefense.ScriptableObjects;
using NeonDefense.Core;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private List<WaveConfig> waves;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float timeBetweenWaves = 5f;
        [SerializeField] private bool autoStart = false;

        private int currentWaveIndex = 0;
        private int activeEnemiesCount = 0;
        private bool isWaveActive = false;
        private bool isSpawning = false;

        private void OnEnable()
        {
            GameEvents.OnEnemyKilled += HandleEnemyDestroyed;
            GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= HandleEnemyDestroyed;
            GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
        }

        private void Start()
        {
            if (autoStart && waves != null && waves.Count > 0)
            {
                StartWave();
            }
        }

        public void StartWave()
        {
            if (isWaveActive || currentWaveIndex >= waves.Count) return;

            isWaveActive = true;
            isSpawning = true;
            activeEnemiesCount = 0;
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex);

            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }

        private IEnumerator SpawnWave(WaveConfig waveConfig)
        {
            foreach (var group in waveConfig.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(1f / group.spawnRate);
                }

                yield return new WaitForSeconds(waveConfig.timeBetweenGroups);
            }

            isSpawning = false;
            CheckWaveEndAndTriggerEvent(); // Verificação de segurança pós-spawn
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null || config == null || config.prefab == null) return;

            Enemy newEnemy = EnemyPool.Instance.Get(config.prefab, spawnPoint.position, spawnPoint.rotation);
            newEnemy.config = config; // Initialize enemy with config
            // Note: Waypoints initialization should happen here or inside Enemy script based on a path manager

            activeEnemiesCount++;
        }

        private void HandleEnemyDestroyed(Enemy enemy)
        {
            DecreaseActiveEnemyCount();
        }

        private void HandleEnemyReachedGoal(Enemy enemy, int damage)
        {
            DecreaseActiveEnemyCount();
        }

        private void DecreaseActiveEnemyCount()
        {
            activeEnemiesCount--;
            CheckWaveEndAndTriggerEvent();
        }

        private void CheckWaveEndAndTriggerEvent()
        {
            if (isWaveActive && !isSpawning && activeEnemiesCount <= 0)
            {
                isWaveActive = false;
                GameEvents.OnWaveEnd?.Invoke(currentWaveIndex);
                UpdateWaveIndexAndScheduleNext();
            }
        }

        private void UpdateWaveIndexAndScheduleNext()
        {
            currentWaveIndex++;
            if (currentWaveIndex < waves.Count)
            {
                Invoke(nameof(StartWave), timeBetweenWaves);
            }
            else
            {
                // All waves completed - Trigger Win State (managed by GameManager typically)
                Debug.Log("All Waves Completed!");
            }
        }
    }
}
