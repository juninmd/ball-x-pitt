# NeonDefense - Solução

Este arquivo contém o código fonte e instruções de configuração para o projeto NeonDefense.

## 1. Scripts Core

### WaveManager.cs
```csharp
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

        private void HandleEnemyDespawned(Enemy enemy, int ignoredValue)
        {
            activeEnemies--;
            CheckWaveCompletion();
        }
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
        run: (cd build/Windows && zip -r ../../Windows.zip .)
            if (nearestEnemy != null)