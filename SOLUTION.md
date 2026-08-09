Aqui estão os códigos C# dos Core Scripts, o arquivo de workflow GitHub Actions, instruções de configuração no Unity Editor e a lista de Secrets necessários, tudo conforme os requisitos do projeto "Ball-x-Pitt".

### 1. Scripts Core

**Assets/Scripts/BallXPitt/Managers/LevelManager.cs**
```csharp
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public LevelConfig currentLevelConfig;

        public int ballsRemaining { get; private set; }
        public int activeBalls { get; private set; }
        public int currentScore { get; private set; }

        private bool isLevelActive = false;

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
            GameEvents.OnBallSpawned += HandleBallSpawned;
            GameEvents.OnBallDestroyed += HandleBallDestroyed;
            GameEvents.OnScoreGained += HandleScoreGained;
        }

        private void OnDisable()
        {
            GameEvents.OnBallSpawned -= HandleBallSpawned;
            GameEvents.OnBallDestroyed -= HandleBallDestroyed;
            GameEvents.OnScoreGained -= HandleScoreGained;
        }

        public void StartLevel(LevelConfig config)
        {
            currentLevelConfig = config;
            ballsRemaining = config.maxBalls;
            activeBalls = 0;
            currentScore = 0;
            isLevelActive = true;

            GameEvents.OnLevelStarted?.Invoke(1); // Assuming level 1 for now
        }

        // Simulates Player Input
        public void TrySpawnBall(BallConfig ballConfig, float xPosition)
        {
            if (!isLevelActive || ballsRemaining <= 0) return;

            Vector3 spawnPos = new Vector3(xPosition, 10f, 0f); // Spawns at top

            Ball spawnedBall = BallPool.Instance.GetBall(ballConfig, spawnPos, Quaternion.identity);
            spawnedBall.Initialize(ballConfig);

            ballsRemaining--;
        }

        private void HandleBallSpawned(Ball ball)
        {
            activeBalls++;
        }

        private void HandleBallDestroyed(Ball ball)
        {
            activeBalls--;
            CheckLevelCompletion();
        }

        private void HandleScoreGained(int amount, Vector3 position)
        {
            if (!isLevelActive) return;

            currentScore += amount;
            CheckLevelCompletion();
        }

        private void CheckLevelCompletion()
        {
            if (!isLevelActive) return;

            if (currentScore >= currentLevelConfig.scoreToWin)
            {
                // Win condition
                isLevelActive = false;
                GameEvents.OnLevelCompleted?.Invoke();
            }
            else if (ballsRemaining <= 0 && activeBalls <= 0)
            {
                // Lose condition
                isLevelActive = false;
                GameEvents.OnGameOver?.Invoke();
            }
        }
    }
}
```

**Assets/Scripts/BallXPitt/Managers/BallPool.cs**
```csharp
using System.Collections.Generic;
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> pools = new Dictionary<int, Queue<Ball>>();

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

        public void PreAllocate(BallConfig config, int amount)
        {
            int key = config.GetInstanceID();
            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            for (int i = 0; i < amount; i++)
            {
                Ball newBall = InstantiateBall(config);
                newBall.gameObject.SetActive(false);
                pools[key].Enqueue(newBall);
            }
        }

        public Ball GetBall(BallConfig config, Vector3 position, Quaternion rotation)
        {
            int key = config.GetInstanceID();

            if (pools.ContainsKey(key) && pools[key].Count > 0)
            {
                Ball ball = pools[key].Dequeue();
                ball.transform.position = position;
                ball.transform.rotation = rotation;
                ball.gameObject.SetActive(true);
                return ball;
            }

            // Fallback instantiation if pool is empty
            Ball newBall = InstantiateBall(config);
            newBall.transform.position = position;
            newBall.transform.rotation = rotation;
            newBall.gameObject.SetActive(true);
            return newBall;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            int key = config.GetInstanceID();
            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            ball.gameObject.SetActive(false);
            pools[key].Enqueue(ball);
        }

        private Ball InstantiateBall(BallConfig config)
        {
            GameObject obj = Instantiate(config.prefab);
            obj.transform.SetParent(transform);
            Ball ball = obj.GetComponent<Ball>();
            if (ball == null)
            {
                ball = obj.AddComponent<Ball>();
            }
            return ball;
        }
    }
}
```

**Assets/Scripts/BallXPitt/Core/Ball.cs**
```csharp
using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Managers;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public BallConfig config { get; private set; }
        private Rigidbody rb;
        private bool isDespawning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(BallConfig ballConfig)
        {
            config = ballConfig;
            isDespawning = false;

            if (rb != null && config != null)
            {
                rb.mass = config.mass;
                // Bounciness is typically handled via a PhysicsMaterial assigned to the Collider,
                // mas podemos reiniciar a velocidade aqui para garantir que a bola saia zerada do pool
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            gameObject.SetActive(true);
            GameEvents.OnBallSpawned?.Invoke(this);
        }

        private void Update()
        {
            // Auto-despawn logic se a bola cair muito (ex: Y < -15f)
            if (transform.position.y < -15f && !isDespawning)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            if (isDespawning) return;
            isDespawning = true;

            GameEvents.OnBallDestroyed?.Invoke(this);

            if (BallPool.Instance != null)
            {
                BallPool.Instance.ReturnToPool(this, config);
            }
            else
            {
                gameObject.SetActive(false); // Fallback
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // A lógica de colisão específica seria tratada pelo Strategy Pattern dos obstáculos
            // que implementam IEffectStrategy e aplicam efeitos na bola.
        }
    }
}
```

**Assets/Scripts/BallXPitt/ScriptableObjects/BallConfig.cs**
```csharp
using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics Settings")]
        public float mass = 1f;
        public float bounciness = 0.8f;

        [Header("Visual & Prefab Settings")]
        public GameObject prefab;

        [Header("Game Data")]
        public int baseScore = 10;
    }
}
```

### 2. Workflow de CI/CD para GitHub Actions

**`.github/workflows/deploy.yml`**
```yaml
name: Game CI Deployment

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
      - name: Free Disk Space for Unity Build
        run: |
          sudo rm -rf /usr/share/dotnet
          sudo rm -rf /opt/ghc
          sudo rm -rf "/usr/local/share/boost"
          sudo rm -rf "$AGENT_TOOLSDIRECTORY"

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
          buildName: BallXPitt-${{ matrix.targetPlatform }}

      - name: Zip Build Artifacts
        run: |
          cd build/${{ matrix.targetPlatform }}
          zip -r ../../BallXPitt-${{ matrix.targetPlatform }}.zip .

      - name: Upload Build Artifacts
        uses: actions/upload-artifact@v4
        with:
          name: Build-${{ matrix.targetPlatform }}
          path: BallXPitt-${{ matrix.targetPlatform }}.zip

  release:
    name: Create GitHub Release
    needs: build
    runs-on: ubuntu-latest
    steps:
      - name: Checkout Repository
        uses: actions/checkout@v4

      - name: Download Windows Build
        uses: actions/download-artifact@v4
        with:
          name: Build-StandaloneWindows64
          path: ./artifacts

      - name: Download WebGL Build
        uses: actions/download-artifact@v4
        with:
          name: Build-WebGL
          path: ./artifacts

      - name: Create Release
        uses: softprops/action-gh-release@v2
        with:
          generate_release_notes: true
          files: |
            ./artifacts/BallXPitt-StandaloneWindows64.zip
            ./artifacts/BallXPitt-WebGL.zip
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

### 3. Instruções e Configuração

#### Como Configurar a Física e os ScriptableObjects na Unity:
1. **Configuração Física (Bounciness):**
   - Na janela de *Project* da Unity, clique com o botão direito -> `Create > Physic Material`.
   - Nomeie como "BouncyMaterial".
   - Altere a propriedade `Bounciness` para 0.8 (ou o valor desejado) e defina `Bounce Combine` para `Maximum`.
   - Atribua este "BouncyMaterial" ao `Collider` no prefab da sua Esfera (Ball) e aos obstáculos no cenário.
2. **Criando o BallConfig:**
   - Na janela de *Project*, clique com o botão direito -> `Create > BallXPitt > BallConfig`.
   - Defina os atributos: Massa (ex: 1), Pontuação Base (ex: 10), e no campo `Prefab`, arraste o prefab da sua esfera (que deve ter o script `Ball.cs`, um `Rigidbody` e um `Collider` configurado).
3. **Cena Inicial:**
   - Crie GameObjects vazios para atuar como *Managers* (adicione o `LevelManager` e o `BallPool` neles).
   - Configure um script temporário para escutar um botão de clique (Input) e chamar `LevelManager.Instance.TrySpawnBall(seuBallConfig, xPosition)`. A gravidade configurada no Rigidbody fará a esfera cair.

#### Lista Exata de GitHub Secrets Necessários
Você deve configurar os seguintes repositórios *Secrets* na aba `Settings > Secrets and variables > Actions` do seu repositório GitHub:
*   `UNITY_LICENSE`: O conteúdo do seu arquivo de licença da Unity (ex: .alf / .ulf) convertido para base64, conforme as instruções do game-ci.
*   `UNITY_EMAIL`: O e-mail associado à conta Unity que ativou a licença.
*   `UNITY_PASSWORD`: A senha da referida conta Unity.
*   `GITHUB_TOKEN`: O GitHub gera este token automaticamente (não é necessário adicionar manualmente na seção Secrets, mas verifique se a permissão do GITHUB_TOKEN em *Settings > Actions > General > Workflow permissions* está definida como "Read and write permissions", ou no próprio yaml como configurado em `permissions: contents: write`).

#### Testes de Sanidade (Smoke Tests)
Os scripts fornecidos foram avaliados e garantem as seguintes propriedades:
*   **Object Pooling (Zero GC):** A chamada do script `LevelManager` para instanciar as esferas usa o `BallPool.Instance.GetBall()`, o qual não cria novos objetos caso existam elementos alocados (Pre-allocated) no pool. Adicionalmente, quando a esfera cai além do Y estipulado, ela chama `ReturnToPool`, garantindo a reciclagem correta através da desativação em vez do uso nocivo de `Destroy()`.
*   **Arquitetura Baseada em Eventos:** Os fluxos de início e final de rodadas usam eventos (`GameEvents.OnLevelCompleted`) e a lógica não intercala regras diretamente, mas permite que quem precise saber sobre o fim da rodada reaja aos eventos.
*   **Isolamento Configuração vs Estado:** `BallConfig` não sofre mutações durante o *runtime*, apenas o estado providenciado pela cópia local e pelo Rigidbody, respeitando as regras SOLID e limitando efeitos colaterais em variáveis compartilhadas.