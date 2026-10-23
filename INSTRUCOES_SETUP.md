# Instruções de Setup - Ball-x-Pitt

Este documento contém as instruções solicitadas para configuração do projeto Ball-x-Pitt na Unity e GitHub Actions.

## 1. Como configurar a física básica e os ScriptableObjects

Para testar a primeira queda de bola, você precisa configurar os objetos e a física:

### Configurando o Physics Material 2D
O jogo depende do comportamento elástico (quique) das esferas.
1. Na Unity, crie uma pasta chamada `Assets/Physics` se não existir.
2. Clique com o botão direito `Create > 2D > Physics Material 2D`. Nomeie como `BouncyMaterial`.
3. No Inspector do material, defina:
   - **Friction:** `0.0` (Evita que a bola pare presa).
   - **Bounciness:** `0.8` (Ajuste para o nível de "pula-pula" desejado).
4. Selecione o Prefab da sua Bola (Ball) e no componente `Collider2D`, arraste o `BouncyMaterial` para o campo **Material**.

### Configurando os ScriptableObjects
Os ScriptableObjects mantêm os dados isolados da lógica.
1. **Configuração da Bola (BallConfig):**
   - Vá em `Assets/Configs` e clique `Create > BallXPitt > BallConfig`.
   - Nomeie como `DefaultBallConfig`.
   - No Inspector, ajuste `Mass` (ex: 1.0), `Base Score` (ex: 100), e associe os Prefabs visual e de partículas.
2. **Configuração do Nível (LevelConfig):**
   - Clique `Create > BallXPitt > LevelConfig`. Nomeie como `Level_1`.
   - Defina `Max Balls` (ex: 10), `Score To Win` (ex: 1000) e os limites (`minX`, `maxX`, `spawnHeight`).
3. **Cena Base:**
   - Adicione os Managers (LevelManager, ScoreManager, BallPool) em objetos vazios na cena.
   - Atribua os ScriptableObjects criados aos campos correspondentes no `LevelManager`.

## 2. Segredos do GitHub (GitHub Secrets)

Para que o workflow de CI/CD para Unity (`.github/workflows/deploy.yml`) rode com sucesso via game-ci, você precisa configurar credenciais da sua licença Unity como segredos no seu repositório do GitHub.

Vá até a página do repositório no GitHub > **Settings** > **Secrets and variables** > **Actions** > **New repository secret**.

Adicione **exatamente** esta lista de secrets:

- `UNITY_LICENSE`: O conteúdo texto (XML) do seu arquivo de licença de ativação (`.alf`/`.ulf`).
- `UNITY_EMAIL`: O e-mail da sua conta Unity.
- `UNITY_PASSWORD`: A senha da sua conta Unity.

*(As compilações para WebGL e Windows ocorrerão automaticamente sempre que você criar uma Tag começando com `v`, ex: `v1.0`)*
