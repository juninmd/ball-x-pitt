# Configuração do Ball-X-Pitt

## 1. Configurando a Física Básica e ScriptableObjects
Para testar a primeira queda de bola no Unity Editor:

1. **Physics Material 2D**:
   - Crie um novo `Physics Material 2D` (Botão direito > `Create` > `2D` > `Physics Material 2D`).
   - Defina o `Bounciness` (elasticidade) para um valor alto, como `0.8`, e `Friction` (atrito) para `0` ou `0.1`.

2. **BallConfig**:
   - Crie o ScriptableObject da bola (Botão direito > `Create` > `BallXPitt` > `Ball Config`).
   - Atribua o `Physics Material 2D` recém-criado ao campo `Physics Material`.
   - Defina a `Mass` como `1` e `Bounciness` base.
   - Associe o Prefab visual da bola (o prefab precisa ter um `Rigidbody2D` e um `CircleCollider2D` adicionados, mas a configuração física será injetada em tempo de execução).

3. **LevelConfig**:
   - Crie o ScriptableObject do nível (Botão direito > `Create` > `BallXPitt` > `Level Config`).
   - Defina `spawnHeight` para uma altura no topo da tela (ex: `8`), e limites horizontais `minX` e `maxX` (ex: `-5` e `5`).

4. **Teste na Cena**:
   - Crie um GameObject vazio chamado `Managers` e adicione os scripts `GameManager`, `LevelManager`, `ScoreManager` e `BallPool`.
   - No `LevelManager`, associe o `LevelConfig` criado em `currentLevelConfig` e o `BallConfig` criado em `defaultBallConfig`.
   - Dê "Play" no editor. Clique em qualquer lugar próximo ao topo da tela; a bola deve ser instanciada (usando o pool) e cair aplicando a gravidade e o physics material criado.

## 2. Secrets do GitHub Actions (CI/CD)
Para que o workflow `.github/workflows/deploy.yml` compile o jogo em Windows e WebGL usando `game-ci`, vá na aba **Settings > Secrets and variables > Actions** do seu repositório no GitHub e adicione os seguintes 3 Secrets exatos:

- `UNITY_EMAIL`: O endereço de e-mail da sua conta Unity.
- `UNITY_PASSWORD`: A senha da sua conta Unity.
- `UNITY_LICENSE`: O conteúdo do seu arquivo de licença do Unity (`.ulf`). Você pode obter esse arquivo ativando a licença manualmente em sua máquina (ver documentação do game-ci sobre Activation) e colando o texto XML inteiro aqui.
