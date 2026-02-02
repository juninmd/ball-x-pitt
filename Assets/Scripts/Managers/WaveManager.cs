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

        public void StartNextWave()
        {
            if (!isSpawning && currentWaveIndex < waves.Count)
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            }
        }

        private IEnumerator StartGameRoutine()
        {
            yield return new WaitForSeconds(2f); // Initial delay
            StartNextWave();
        }

        private IEnumerator SpawnWave(WaveConfig waveConfig)
        {
            isSpawning = true;
            GameEvents.OnWaveStart?.Invoke(currentWaveIndex + 1);

            foreach (var group in waveConfig.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyConfig);
                    yield return new WaitForSeconds(group.spawnRate);
                }

                // Wait between groups if needed, though usually handled by next group iteration immediately
                // unless we want specific delays between groups handled in config logic more complexly.
                // WaveConfig has 'timeBetweenGroups' but that might be after the whole wave?
                // The struct has spawnRate. The WaveConfig has timeBetweenGroups.
                // Interpreting timeBetweenGroups as time AFTER this wave before the next, or between groups?
                // The name suggests between groups. Let's assume between groups inside the wave.
                // But usually standard TD is Group 1 finishes spawning, wait, Group 2.
                // For now, I'll just spawn them sequentially.
            }

            isSpawning = false;
            currentWaveIndex++;

            if (currentWaveIndex >= waves.Count)
            {
                // All waves spawned. Wait for enemies to die to declare victory?
                // For now, just log.
                Debug.Log("All waves spawned.");
            }
            else
            {
                 yield return new WaitForSeconds(waveConfig.timeBetweenGroups);
                 StartNextWave(); // Auto start next wave for this simple implementation
            }
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.LogError("EnemyPool missing!");
                return;
            }

            Enemy enemy = EnemyPool.Instance.Get();
            // Assuming the pool sets position/active, but Enemy.Initialize handles specific config
            enemy.Initialize(config, waypoints);
        }
    }
}
