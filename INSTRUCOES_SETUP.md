# Instruções de Setup - Ball-x-Pitt

Este guia explica como configurar o ambiente e a física básica do projeto na Unity, bem como os secrets necessários no GitHub Actions.

## Configuração na Unity Editor

### 1. ScriptableObjects
Para criar os dados necessários para o jogo, utilizaremos os `ScriptableObjects` providenciados.

*   **BallConfig:**
    1.  Clique com o botão direito na aba *Project*.
    2.  Vá em `Create -> BallXPitt -> BallConfig`.
    3.  Selecione o arquivo recém-criado. No *Inspector*, preencha a Massa (Mass), selecione o Prefab da bola e do efeito de colisão visual (ParticleSystem), além da Pontuação Base (Base Score).
*   **LevelConfig:**
    1.  Clique com o botão direito na aba *Project*.
    2.  Vá em `Create -> BallXPitt -> LevelConfig`.
    3.  No *Inspector*, preencha as variáveis de limite de bolas (`maxBalls`), a pontuação necessária para vencer (`scoreToWin`) e, opcionalmente, defina as coordenadas X e Y onde as bolas deverão aparecer na parte superior da tela.

### 2. Configuração de Física Básica (Physics Material 2D)
Para que a bola quique de forma correta ao atingir as paredes ou os Bumpers:

1.  Crie um novo Material Físico: Clique com o botão direito na aba *Project* -> `Create -> 2D -> Physics Material 2D`.
2.  Nomeie como `BouncyMaterial` ou algo similar.
3.  Selecione o material. No *Inspector*, ajuste o valor de **Bounciness** (geralmente entre `0.6` e `0.9` funciona melhor para efeito "pinball"). Defina **Friction** como `0` ou num valor baixo.
4.  Arraste este material para o componente `Collider 2D` (como um `CircleCollider2D`) no **Prefab da Bola** e também nos colisores dos **Bumpers** ou **Paredes** do "Pit".

### 3. Setup da Cena Base
*   Certifique-se de que há instâncias do `LevelManager`, `ScoreManager`, `BallPool`, `GameManager` e `BallFactory` ativos na cena principal.
*   Associe a `BallConfig` padrão que será usada no `LevelManager` e alimente os `LevelConfig`.

---

## Configuração do GitHub Actions (CI/CD)

Para que o pipeline de integração contínua (game-ci) funcione perfeitamente, você deve adicionar os seguintes **Secrets** no repositório GitHub (`Settings -> Secrets and variables -> Actions`):

*   `UNITY_LICENSE`: O conteúdo do arquivo de licença `.ulf` da Unity.
*   `UNITY_EMAIL`: O e-mail da sua conta Unity (utilizado na ativação da licença).
*   `UNITY_PASSWORD`: A senha da sua conta Unity.
