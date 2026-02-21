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
```

### Tower.cs
```csharp
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    /// <summary>
    /// Base class for all towers. Handles targeting logic and delegates attack execution to an IAttackStrategy.
    /// Follows the Strategy Pattern to allow dynamic attack behaviors (Laser, Missile, etc.).
    /// </summary>
    public class Tower : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("The configuration scriptable object defining stats and strategy.")]
        [SerializeField] private TowerConfig config;

        [Tooltip("The transform point from which projectiles/attacks originate.")]
        [SerializeField] private Transform firePoint;

        [Tooltip("LayerMask to filter enemies during targeting.")]
        [SerializeField] private LayerMask enemyLayer;

        private IAttackStrategy attackStrategy;
        private float fireCountdown = 0f;
        private Enemy currentTarget;

        // Pre-allocated buffer for OverlapSphereNonAlloc to avoid GC allocations during Update
        private readonly Collider[] hitBuffer = new Collider[20];

        /// <summary>
        /// Initializes the tower with a specific configuration and strategy.
        /// Useful for Factory creation or runtime upgrades.
        /// </summary>
        /// <param name="config">The tower configuration.</param>
        /// <param name="strategy">The specific attack strategy implementation.</param>
        public void Initialize(TowerConfig config, IAttackStrategy strategy)
        {
            this.config = config;
            this.attackStrategy = strategy;
            this.fireCountdown = 0f;
        }

        private void Start()
        {
            // Fallback: If placed in editor without Factory, try to self-initialize based on config
            if (config != null && attackStrategy == null)
            {
                InitializeStrategyFromConfig();
            }

            if (firePoint == null)
            {
                firePoint = transform;
            }
        }

        /// <summary>
        /// Creates the appropriate strategy based on the TowerConfig enum.
        /// Acts as a local factory if the Strategy wasn't injected.
        /// </summary>
        private void InitializeStrategyFromConfig()
        {
            switch (config.strategyType)
            {
                case AttackStrategyType.Laser:
                    attackStrategy = new LaserAttackStrategy();
                    break;
                case AttackStrategyType.Missile:
                    attackStrategy = new MissileAttackStrategy();
                    break;
                // Extend with more cases as needed
                default:
                    Debug.LogWarning($"Unknown strategy type: {config.strategyType}. Defaulting to Laser.");
                    attackStrategy = new LaserAttackStrategy();
                    break;
            }
        }

        private void Update()
        {
            if (config == null) return;

            UpdateTarget();

            if (currentTarget != null)
            {
                if (fireCountdown <= 0f)
                {
                    Attack();
                    fireCountdown = 1f / config.fireRate;
                }
            }

            fireCountdown -= Time.deltaTime;
        }

        /// <summary>
        /// Finds the nearest enemy within range using non-allocating physics overlap.
        /// </summary>
        private void UpdateTarget()
        {
            // Efficiency: Search for enemies within range using a non-allocating Physics call
            // Clears previous buffer content implicitly by overwriting with new count
            int count = Physics.OverlapSphereNonAlloc(transform.position, config.range, hitBuffer, enemyLayer);

            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < count; i++)
            {
                Collider hit = hitBuffer[i];
                if (hit == null) continue;

                // Optimization: Check for component.
                // Using TryGetComponent avoids garbage allocation in newer Unity versions compared to GetComponent
                if (hit.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemyComponent;
                    }
                }
            }

            // Update target if valid and within range
            if (nearestEnemy != null && shortestDistance <= config.range)
            {
                currentTarget = nearestEnemy;
            }
            else
            {
                currentTarget = null;
            }
        }

        private void Attack()
        {
            if (attackStrategy != null && currentTarget != null)
            {
                attackStrategy.Attack(currentTarget, firePoint, config);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (config != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, config.range);
            }
        }
    }
}
```

### ProjectilePool.cs
```csharp
using UnityEngine;

namespace NeonDefense.Core
{
    /// <summary>
    /// Singleton Object Pool specifically for Projectiles.
    /// Manages reusable projectile instances to avoid Garbage Collection (GC) spikes during intense combat.
    /// Implements the Object Pooling Pattern requirement.
    /// </summary>
    [DisallowMultipleComponent]
    public class ProjectilePool : ObjectPool<Projectile>
    {
        public static ProjectilePool Instance { get; private set; }

        protected override void Awake()
        {
            // Ensure Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Initialize the pool from the base class (pre-warms the pool)
            base.Awake();
        }
    }
}
```

### EnemyConfig.cs
```csharp
using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.ScriptableObjects
{
    /// <summary>
    /// Configuration data for an Enemy type.
    /// Stores base attributes like Health, Speed, and Rewards.
    /// Allows designers to tweak values without touching code.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("General")]
        [Tooltip("The name of the enemy type.")]
        public string enemyName;

        [Tooltip("The prefab to spawn for this enemy.")]
        public Enemy prefab;

        [Header("Stats")]
        [Tooltip("Health points of the enemy.")]
        [Range(1f, 10000f)]
        public float health = 10f;

        [Tooltip("Movement speed in units per second.")]
        [Range(0.1f, 50f)]
        public float speed = 5f;

        [Tooltip("Currency awarded to player on death (Bits).")]
        [Range(1, 1000)]
        public int bitDrop = 10;

        [Tooltip("Damage dealt to player/core upon reaching the goal.")]
        [Range(1, 1000)]
        public int damageToPlayer = 1;
    }
}
```

## 2. Workflow (GitHub Actions)

### .github/workflows/deploy.yml
```yaml
# DevOps Workflow for NeonDefense
# Triggers on tags starting with 'v' (e.g., v1.0, v1.1)
# Builds for Windows 64-bit and WebGL
# Creates a GitHub Release with zipped artifacts
name: Deploy

on:
  push:
    tags:
      - 'v*'

permissions:
  contents: write

jobs:
  build:
    name: Build for ${{ matrix.targetPlatform }}
    runs-on: ubuntu-latest
    strategy:
      fail-fast: false
      matrix:
        targetPlatform:
          - StandaloneWindows64
          - WebGL
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          fetch-depth: 0
          lfs: true

      - name: Cache Library
        uses: actions/cache@v4
        with:
          path: Library
          key: Library-${{ matrix.targetPlatform }}-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
          restore-keys: |
            Library-${{ matrix.targetPlatform }}-
            Library-

      # Build Step using game-ci/unity-builder
      # License Treatment:
      # The builder automatically activates Unity using the provided environment variables.
      # Required Secrets:
      # - UNITY_LICENSE: Content of the .ulf file (Recommended for stability)
      # OR
      # - UNITY_EMAIL & UNITY_PASSWORD: For manual activation (Less stable on CI)
      - name: Build project
        uses: game-ci/unity-builder@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          targetPlatform: ${{ matrix.targetPlatform }}

      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: Build-${{ matrix.targetPlatform }}
          path: build/${{ matrix.targetPlatform }}

  release:
    name: Create Release
    needs: build
    runs-on: ubuntu-latest
    steps:
      - name: Download Windows Artifact
        uses: actions/download-artifact@v4
        with:
          name: Build-StandaloneWindows64
          path: build/Windows

      - name: Download WebGL Artifact
        uses: actions/download-artifact@v4
        with:
          name: Build-WebGL
          path: build/WebGL

      - name: Zip Windows Build
        run: zip -r Windows.zip build/Windows

      - name: Zip WebGL Build
        run: zip -r WebGL.zip build/WebGL

      - name: Create Release
        uses: softprops/action-gh-release@v1
        with:
          files: |
            Windows.zip
            WebGL.zip
          generate_release_notes: true
          draft: false
          prerelease: false
```

## 3. Instruções de Configuração

### Configuração do ScriptableObject (EnemyConfig)
1. Na janela Project, vá para `Assets/Scripts/ScriptableObjects/`.
2. Clique com o botão direito -> **Create -> NeonDefense -> EnemyConfig**.
3. Nomeie como `BasicVirus`.
4. Configure no Inspector:
   - **Prefab**: Arraste seu prefab de Inimigo.
   - **Health**: 10
   - **Speed**: 3
   - **Bit Drop**: 5

### Configuração da Onda (WaveConfig)
1. Clique com o botão direito -> **Create -> NeonDefense -> WaveConfig**.
2. Adicione grupos de inimigos usando o `BasicVirus` criado acima.

### Segredos do GitHub (Secrets)
No seu repositório GitHub, vá em **Settings -> Secrets and variables -> Actions** e adicione:

| Nome | Descrição |
| :--- | :--- |
| `UNITY_LICENSE` | Conteúdo do arquivo `.ulf` (Recomendado). |
| `UNITY_EMAIL` | Seu email da Unity ID. |
| `UNITY_PASSWORD` | Sua senha da Unity ID. |
