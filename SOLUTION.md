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

      - name: Cache Library
        uses: actions/cache@v4
        with:
            if (nearestEnemy != null)
          key: Library-${{ matrix.targetPlatform }}-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
          restore-keys: |
            Library-${{ matrix.targetPlatform }}-
            Library-

            if (nearestEnemy != null)
      # License Treatment:
      # The builder automatically activates Unity using the provided environment variables.
      # Required Secrets:
      # - UNITY_LICENSE: Content of the .ulf file (Recommended for stability)
      # OR
            if (nearestEnemy != null)
      - name: Build project
        uses: game-ci/unity-builder@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
            if (nearestEnemy != null)
        with:
          targetPlatform: ${{ matrix.targetPlatform }}

      - name: Upload artifact
        uses: actions/upload-artifact@v4
            if (nearestEnemy != null)
          name: Build-${{ matrix.targetPlatform }}
          path: build/${{ matrix.targetPlatform }}

  release:
    name: Create Release
            if (nearestEnemy != null)
    runs-on: ubuntu-latest
    steps:
      - name: Download Windows Artifact
        uses: actions/download-artifact@v4
        with:
            if (nearestEnemy != null)
          path: build/Windows

      - name: Download WebGL Artifact
        uses: actions/download-artifact@v4
        with:
            if (nearestEnemy != null)
          path: build/WebGL

      - name: Zip Windows Build
        run: (cd build/Windows && zip -r ../../Windows.zip .)

            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)

      - name: Create Release
        run: (cd build/Windows && zip -r ../../Windows.zip .)
        with:
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
            WebGL.zip
          generate_release_notes: true
        run: (cd build/Windows && zip -r ../../Windows.zip .)
          prerelease: false
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
## 3. Instruções de Configuração

        run: (cd build/Windows && zip -r ../../Windows.zip .)
1. Na janela Project, vá para `Assets/Scripts/ScriptableObjects/`.
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
4. Configure no Inspector:
   - **Prefab**: Arraste seu prefab de Inimigo.
        run: (cd build/Windows && zip -r ../../Windows.zip .)
   - **Speed**: 3
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)
### Configuração da Onda (WaveConfig)
1. Clique com o botão direito -> **Create -> NeonDefense -> WaveConfig**.
        run: (cd build/Windows && zip -r ../../Windows.zip .)

            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)

| Nome | Descrição |
        run: (cd build/Windows && zip -r ../../Windows.zip .)
| `UNITY_LICENSE` | Conteúdo do arquivo `.ulf` (Recomendado). |
            if (nearestEnemy != null)
        run: (cd build/WebGL && zip -r ../../WebGL.zip .)

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