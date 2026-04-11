# NeonDefense - Tower Defense Cyberpunk

Aqui estão os artefatos solicitados para o seu jogo Tower Defense, seguindo as diretrizes de Clean Code, SOLID e os Design Patterns exigidos (Object Pooling, Factory, Strategy).

## 1. Scripts Core

### `WaveManager.cs` (Controle de Ondas)
Gerencia o fluxo de ondas, spawn de inimigos e se comunica via Eventos (C# Actions).

```csharp
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
                    yield return new WaitForSeconds(group.spawnRate > 0 ? 1f / group.spawnRate : 1f);
                }

                yield return new WaitForSeconds(waveConfig.timeBetweenGroups);
            }

            isSpawning = false;
            CheckWaveEndAndTriggerEvent();
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            if (EnemyPool.Instance == null || config == null || config.prefab == null) return;

            Enemy newEnemy = EnemyPool.Instance.Get(config.prefab, spawnPoint.position, spawnPoint.rotation);
            newEnemy.config = config;

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
                Debug.Log("All Waves Completed!");
            }
        }
    }
}
```

### `Tower.cs` (Base da Torre)
A base da torre utiliza o **Strategy Pattern** para delegar o comportamento de ataque (`IAttackStrategy`). A busca por alvos utiliza `Physics.OverlapSphereNonAlloc` para garantir Zero GC (sem alocações durante a gameplay) e é executada através de uma Coroutine a cada 0.2s para poupar CPU.

```csharp
using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;
using System.Collections;

namespace NeonDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayerMask;

        private TowerConfig config;
        private IAttackStrategy attackStrategy;
        private Enemy currentTarget;
        private float fireCountdown = 0f;

        private Collider[] targetBuffer = new Collider[20];

        public void Initialize(TowerConfig towerConfig, IAttackStrategy strategy)
        {
            this.config = towerConfig;
            this.attackStrategy = strategy;

            StartCoroutine(UpdateTarget());
        }

        private IEnumerator UpdateTarget()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                FindTarget();
                yield return wait;
            }
        }

        private void FindTarget()
        {
            if (currentTarget != null)
            {
                if (!currentTarget.gameObject.activeInHierarchy ||
                    (transform.position - currentTarget.transform.position).sqrMagnitude > config.range * config.range)
                {
                    currentTarget = null;
                }
                else
                {
                    return;
                }
            }

            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, config.range, targetBuffer, enemyLayerMask);

            float shortestDistanceSqr = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < numColliders; i++)
            {
                Enemy enemy = targetBuffer[i].GetComponentInParent<Enemy>();
                if (enemy != null && enemy.gameObject.activeInHierarchy)
                { 
                    float distanceSqr = (transform.position - enemy.transform.position).sqrMagnitude;
                    if (distanceSqr < shortestDistanceSqr)
                    { 
                        shortestDistanceSqr = distanceSqr;
                        nearestEnemy = enemy;
                    }
                }
            }

            currentTarget = nearestEnemy;
        }

        private void Update()
        {
            if (currentTarget == null || config == null || attackStrategy == null) return;

            fireCountdown -= Time.deltaTime;

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = config.fireRate > 0 ? 1f / config.fireRate : 1f;
            }
        }

        private void Shoot()
        {
            attackStrategy.Attack(currentTarget, firePoint, config);
        }

        private void OnDrawGizmosSelected()
        {
            if (config != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, config.range);
            }
        }
    }
}
```

### `ProjectilePool.cs` (Object Pooling)
Implementação de um Pool Global de projéteis para evitar GC via instanciação e destruição dinâmica. (Herda de `ObjectPool<T>`).

```csharp
using UnityEngine;

namespace NeonDefense.Core
{
    [DisallowMultipleComponent]
    public class ProjectilePool : ObjectPool<Projectile>
    {
        public static ProjectilePool Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                base.Awake();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
```

### `EnemyConfig.cs` (ScriptableObject)
Define os dados e configuração de cada Inimigo de forma separada da lógica. Restrições via `[Range]` facilitam o trabalho do Game Designer.

```csharp
using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/Enemy Config", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [Tooltip("The Enemy prefab to instantiate. Must contain the Enemy component.")]
        public Enemy prefab;

        [Header("Stats")]
        [Range(1f, 10000f)]
        public float health = 100f;

        [Range(0.1f, 50f)]
        public float speed = 5f;

        [Range(1, 1000)]
        public int bitDrop = 10;

        [Range(1, 1000)]
        public int damageToPlayer = 1;
    }
}
```

---

## 2. Workflow CI/CD (`.github/workflows/deploy.yml`)

Este arquivo automatiza o build para Windows 64 e WebGL, rodando **apenas** quando uma Tag de versão (`v*`) é criada e também cria uma Release do GitHub.

```yaml
name: Deploy Unity Project

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
      - name: Checkout Repository
        uses: actions/checkout@v4
        with:
          lfs: true

      - name: Cache Unity Library
        uses: actions/cache@v4
        with:
          path: Library
          key: Library-${{ matrix.targetPlatform }}-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
          restore-keys: |
            Library-${{ matrix.targetPlatform }}-
            Library-

      - name: Build Unity Project
        uses: game-ci/unity-builder@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          targetPlatform: ${{ matrix.targetPlatform }}
          buildName: NeonDefense
          versioning: Tag

      - name: Upload Build Artifacts
        uses: actions/upload-artifact@v4
        with:
          name: Build-${{ matrix.targetPlatform }}
          path: build/${{ matrix.targetPlatform }}

  release:
    name: Create GitHub Release
    needs: build
    runs-on: ubuntu-latest
    steps:
      - name: Checkout Repository
        uses: actions/checkout@v4

      - name: Download Windows Artifact
        uses: actions/download-artifact@v4
        with:
          name: Build-StandaloneWindows64
          path: builds/Windows

      - name: Download WebGL Artifact
        uses: actions/download-artifact@v4
        with:
          name: Build-WebGL
          path: builds/WebGL

      - name: Zip Windows Build
        run: zip -r NeonDefense-Windows.zip builds/Windows/

      - name: Zip WebGL Build
        run: zip -r NeonDefense-WebGL.zip builds/WebGL/

      - name: Create Release and Upload Assets
        uses: softprops/action-gh-release@v2
        with:
          generate_release_notes: true
          files: |
            NeonDefense-Windows.zip
            NeonDefense-WebGL.zip
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

---

## 3. Instruções

### Como configurar os ScriptableObjects no Editor para criar a primeira onda:

1. **Criando o Inimigo (Vírus):**
   - Na janela **Project**, clique com o botão direito e navegue até `Create > NeonDefense > Enemy Config`.
   - Dê um nome ao arquivo, como `BasicVirus`.
   - Selecione-o e, no **Inspector**, arraste o Prefab do seu inimigo para o campo `Prefab`. Ajuste os atributos como preferir (ex: Vida = 100, Speed = 5).

2. **Criando a Onda:**
   - Clique novamente com o botão direito na aba **Project**, e vá em `Create > NeonDefense > Wave Config`.
   - Dê o nome de `Wave_01`.
   - No **Inspector**, no array `Enemy Groups`, adicione um novo elemento.
   - Arraste o `BasicVirus` para o campo `Enemy Config` deste elemento. Configure `Count` para o número de inimigos (ex: 10) e `Spawn Rate` para a taxa de aparição por segundo.

3. **Configurando a Cena:**
   - Selecione o GameObject que contém o seu componente `WaveManager` (geralmente um objeto vazio chamado `GameManagers`).
   - No Inspector, no array `Waves`, arraste a `Wave_01` criada.
   - Ative `Auto Start` caso queira que a onda inicie automaticamente.

### Lista Exata dos Secrets para Adicionar no GitHub:
Vá em **Settings > Secrets and variables > Actions > New repository secret** e adicione os 3 secrets a seguir:

- **`UNITY_EMAIL`**: O e-mail da sua conta Unity (ex: email@exemplo.com).
- **`UNITY_PASSWORD`**: A senha da sua conta Unity.
- **`UNITY_LICENSE`**: O conteúdo do arquivo de licença `.ulf` gerado (para Personal, é necessário extrair a licença usando o Unity Hub local ou seguindo a documentação do Game-CI em [Activation](https://game-ci/docs/github/activation) e colar o conteúdo XML diretamente neste Secret).
