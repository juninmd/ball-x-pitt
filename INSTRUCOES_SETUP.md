# Instruções de Setup - Ball-x-Pitt

Bem-vindo ao projeto **Ball-x-Pitt**! Siga as instruções abaixo para configurar a física básica, os ScriptableObjects e a pipeline de CI/CD (GitHub Actions).

## 1. Configurando a Física Básica na Unity

O jogo "Ball-x-Pitt" é um arcade baseado em física. Para que as bolas (Balls) quiquem corretamente, precisamos configurar um **Physics Material 2D**.

1. Na aba **Project** da Unity, navegue até a pasta `Assets/Settings` ou crie uma pasta `Assets/Physics`.
2. Clique com o botão direito `Create > 2D > Physics Material 2D`.
3. Nomeie o material como `BouncyMaterial`.
4. Selecione o `BouncyMaterial` criado e vá na aba **Inspector**:
   - Defina o campo **Friction** como `0` (para a bola não "agarrar" nas paredes).
   - Defina o campo **Bounciness** entre `0.6` e `0.9` (quanto maior, mais a bola vai quicar).

## 2. Configurando ScriptableObjects no Editor

Usamos ScriptableObjects para separar os dados de configuração da lógica. O jogo precisa de configurações para as bolas e para os níveis.

### A. Criando um `BallConfig`
1. Navegue até a pasta `Assets/Scripts/BallXPitt/ScriptableObjects` (ou outra pasta de sua preferência para dados).
2. Clique com o botão direito `Create > BallXPitt > Ball Config`.
3. Nomeie o arquivo (ex: `DefaultBall`).
4. Selecione o arquivo e, no **Inspector**:
   - **Mass:** Ajuste a massa da bola (ex: `1`).
   - **Bounciness:** Pode deixar como `0.8` (este valor é usado em alguns cálculos).
   - **Physics Material:** Arraste o `BouncyMaterial` que criamos no passo anterior para cá.
   - **Prefab:** Crie um Prefab com um GameObject que possua SpriteRenderer, Rigidbody2D, CircleCollider2D e o script `Ball.cs`. Arraste esse prefab para este campo.
   - **Collision VFX Prefab:** (Opcional) Arraste um prefab de ParticleSystem.
   - **Base Score:** A pontuação base que a bola dá (ex: `10`).

### B. Criando um `LevelConfig`
1. Na mesma pasta, clique com o botão direito `Create > BallXPitt > Level Config`.
2. Nomeie o arquivo (ex: `Level1`).
3. Selecione o arquivo e, no **Inspector**:
   - **Level Id:** `1`
   - **Max Balls:** Número inicial de bolas (ex: `10`).
   - **Score To Win:** Pontuação para vencer (ex: `1000`).
   - **Layout Prefab:** Arraste um Prefab que contenha os obstáculos (`BumperBounceEffect`, `ScoreMultiplierEffect`, etc.) dessa fase.
   - **Spawn Height:** A altura no eixo Y onde a bola vai surgir.
   - **Min X / Max X:** O limite horizontal de onde o jogador pode lançar a bola no topo da tela.

### C. Atribuindo os Configs no GameManager e LevelManager
1. Na sua Scene, encontre o GameObject que possui o script `GameManager` e atribua o `LevelConfig` recém-criado no campo **Initial Level**.
2. Encontre o GameObject que possui o script `LevelManager` e atribua o `BallConfig` no campo **Default Ball Config**.

## 3. Configurando a Pipeline de CI/CD no GitHub (Game-CI)

Para que os builds automatizados para Windows 64-bit e WebGL funcionem, você precisa adicionar as credenciais da sua conta Unity nos **Secrets** do repositório no GitHub.

1. Acesse seu repositório no GitHub.
2. Vá em **Settings > Secrets and variables > Actions**.
3. Clique em **New repository secret**.
4. Adicione os seguintes Secrets exatamente com estes nomes:

- `UNITY_EMAIL` : O endereço de email usado na sua conta Unity.
- `UNITY_PASSWORD` : A senha da sua conta Unity.
- `UNITY_LICENSE` : O conteúdo do seu arquivo de licença da Unity (formato `.alf` convertido para `.ulf` via ativação manual). [Siga este tutorial oficial do Game-CI para gerar o UNITY_LICENSE](https://game.ci/docs/github/activation/).

### Disparando o Build Automático
A pipeline configurada em `.github/workflows/deploy.yml` será disparada **APENAS** quando você criar uma Tag no Git que comece com a letra "v".

Exemplo via linha de comando:
```bash
git tag v1.0.0
git push origin v1.0.0
```

Após o build, a pipeline criará automaticamente uma **Release** no GitHub com o changelog e anexará os arquivos `Ball-X-Pitt-Windows.zip` e `Ball-X-Pitt-WebGL.zip`.
