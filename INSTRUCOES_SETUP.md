<!-- Instruções de Setup -->
# Ball-x-Pitt: Instruções de Setup e Documentação

## Arquitetura e Visão Geral
O projeto **Ball-x-Pitt** é um jogo arcade baseado em física no estilo Pachinko/Ball Pit. O objetivo é lançar esferas (Balls) no topo de um cenário vertical (Pit), onde quicam em obstáculos, ativam efeitos variados (multiplicadores, destruição, bônus de pontuação) e caem em zonas ao fundo para somar pontos.

Para manter o código limpo, escalável e evitar gargalos de performance, implementamos os seguintes padrões arquiteturais:

### Princípios SOLID & Clean Code
- **Separação de Responsabilidades:** A lógica de estado global é gerenciada por `GameManager`, a pontuação e seus multiplicadores por `ScoreManager`, e as regras da fase (como o limite de bolas lançadas) por `LevelManager`.
- **Event-Driven:** A comunicação entre sistemas é totalmente desacoplada através do `GameEvents.cs`. Por exemplo, quando uma bola é gerada, ela dispara um `OnBallSpawned`, permitindo que os Managers computem estatísticas sem que a bola precise saber sobre a existência deles.

### Design Patterns Aplicados
- **Object Pooling:** Essencial para evitar **GC Spikes** (falhas e travamentos devidos ao Garbage Collector). `BallPool.cs` gerencia o reaproveitamento das esferas e dos efeitos de partículas de colisão.
- **Factory Method:** `BallFactory.cs` é encarregada da lógica de criar ou buscar a bola certa do Pool antes de habilitá-la na cena.
- **Strategy Pattern:** Utilizado nos comportamentos dos obstáculos (ex: `BumperBounceEffect`, `ScoreMultiplierEffect`). A bola simplesmente delega a responsabilidade de "o que acontece na colisão" para quem a atingiu.
- **ScriptableObjects:** Separação estrita dos dados (`BallConfig`, `LevelConfig`) das lógicas físicas e visuais.

---

## Instruções de Configuração no Unity Editor

Siga este passo a passo para testar a mecânica base (física) e preparar a cena inicial para a primeira queda de bola:

### 1. Criando os Scriptable Objects de Configuração
Você precisará criar instâncias dos ScriptableObjects contendo os atributos base do jogo.
- No Project, clique com o botão direito `Create > BallXPitt > BallConfig`. Chame de `DefaultBallConfig`.
  - No inspector, ajuste a `Mass` (ex: `1.0`) e defina a `BaseScore` (ex: `100`).
  - Arraste o seu Prefab visual de bola e o Prefab de partículas de colisão para os campos respectivos.
- Crie também um `LevelConfig` (`Create > BallXPitt > LevelConfig`).
  - Preencha `MaxBalls`, `ScoreToWin`, além dos limites horizontais (`minX`, `maxX`) e altura do Spawn (`spawnHeight`).

### 2. Configurando a Física (Physics Material 2D)
A propriedade de bounciness (o "pulo" da bola ao bater nos obstáculos) é gerenciada direto pela engine de física da Unity.
- Crie um novo material: `Create > 2D > Physics Material 2D` (nomeie como `BouncyMaterial`).
- No Inspector deste material, ajuste a `Bounciness` para `0.8` (ou outro valor que se adeque ao design do jogo) e defina o `Friction` bem baixo para evitar que a bola pare presa (ex: `0.0`).
- No Prefab da sua Bola (`Ball`), encontre o componente `CircleCollider2D` (ou similar) e arraste o `BouncyMaterial` para o campo `Material`.

### 3. Ajustando os Managers na Cena
- Crie um GameObject vazio na sua cena chamado `Managers`.
- Anexe o script `LevelManager.cs` e arraste os ScriptableObjects (`LevelConfig` e `BallConfig`) recém-criados para seus respectivos campos no Inspector.
- Anexe também os scripts `ScoreManager.cs` e `BallPool.cs`.
- Crie um chão estático com colisor fora da tela ou defina o auto-despawn por eixo Y (já configurado no script da `Ball.cs` para valores `< -15f`).

Dê `Play` no Editor, clique com o mouse no topo da tela e você verá a bola instanciar, cair aplicando física e ser reciclada ao sair dos limites ou por outros gatilhos!

---

## CI/CD: Pipeline de Produção (GitHub Actions)
O fluxo CI/CD está implementado via Game-CI, para automatizar o build para Windows 64-bit e WebGL assim que uma nova versão (tag `v*`) for publicada, gerando uma Release automática.

Para que a compilação funcione, as credenciais da licença da Unity são obrigatórias nos Secrets do GitHub.

### Secrets Necessários no GitHub
Vá em `Settings > Secrets and variables > Actions > New repository secret` e adicione exatamente estes nomes:

- `UNITY_LICENSE`: O conteúdo completo do seu arquivo de licença de ativação (`.alf`/`.ulf`).
- `UNITY_EMAIL`: O e-mail da sua conta registrada na Unity.
- `UNITY_PASSWORD`: A senha da sua conta registrada na Unity.

*(Nota: Estes secrets são confidenciais e nunca devem ser versionados em texto plano no repositório.)*