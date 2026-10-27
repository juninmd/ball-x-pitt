# Instruções de Setup para Ball-x-Pitt

Bem-vindo ao projeto **Ball-x-Pitt**! Este documento explica como configurar as físicas e dependências no Unity e detalha a configuração necessária do GitHub Actions.

## 1. Configurando a Física Básica (Physics Material)

Como este é um jogo focado em simulação física (Pachinko/Ball Pit), a mecânica de rebater ("quicar") é essencial:

1. No Unity Editor, clique com o botão direito na aba **Project** (em uma pasta como `Assets/Settings/Physics` ou semelhante).
2. Selecione **Create > 2D > Physics Material 2D**.
3. Nomeie este material como `BouncyMaterial` (ou algo similar).
4. Selecione o novo arquivo. No **Inspector**, configure a propriedade **Bounciness** para um valor alto, como `0.8` ou `0.9` (varia de 0 a 1).
5. Certifique-se de que a opção **Friction** esteja baixa, por exemplo, `0.1`, para evitar que as bolas fiquem presas nos obstáculos.
6. Associe este material:
   - Ao `Rigidbody2D` ou `Collider2D` do Prefab da sua **Ball**.
   - Aos `Collider2D` dos objetos do cenário (Bumper, Paredes, etc) para que a colisão mantenha a energia cinética adequada.

## 2. Configurando ScriptableObjects (BallConfig e LevelConfig)

Os `ScriptableObjects` permitem configurar os atributos das bolas e dos níveis de forma desacoplada:

1. **BallConfig** (Configuração da Bola)
   - Clique com o botão direito na pasta do seu projeto.
   - Vá em **Create > BallXPitt > BallConfig**.
   - No Inspector, configure:
     - **Mass**: Ex: `1.5`
     - **Prefab**: Assine o GameObject/Prefab da sua Esfera.
     - **Collision VFX Prefab**: Associe o sistema de partículas de explosão/impacto.
     - **Base Score**: Pontos que ela rende por impacto, Ex: `100`.

2. **LevelConfig** (Configuração do Nível)
   - Vá em **Create > BallXPitt > LevelConfig**.
   - No Inspector, configure:
     - **Max Balls**: O número total de bolas permitidas nesta fase, Ex: `50`.
     - **Score To Win**: Pontuação alvo, Ex: `5000`.
     - Defina também os limites de Spawn (`Min X`, `Max X` e `Spawn Height`) para onde o jogador pode mirar na borda superior da tela.

Após criá-los, certifique-se de associar seu `LevelConfig` e `BallConfig` diretamente aos `Managers` na cena (como o `GameManager` e o `LevelManager`).

## 3. Configuração de Secrets no GitHub (Workflow CI/CD)

O arquivo `.github/workflows/deploy.yml` gera a compilação do projeto para Windows (StandaloneWindows64) e WebGL sempre que você lançar uma release na forma de uma _Tag_ iniciada em `v` (ex: `v1.0`).
Para que a Unity possa "buildar" pelo Game-CI, é obrigatório ativar a licença adicionando os seguintes **Secrets** nas configurações do seu repositório no GitHub:

Acesse: **Settings > Secrets and variables > Actions > New repository secret**.

- `UNITY_LICENSE`: O conteúdo do seu arquivo de licença da Unity `.alf` ou `.ulf` (Unity License File). Siga o [Guia oficial do Game-CI](https://game.ci/docs/github/activation) se precisar gerar um.
- `UNITY_EMAIL`: O e-mail da sua conta Unity vinculada a essa licença.
- `UNITY_PASSWORD`: A senha da sua conta Unity.

## 4. Efetuando o Primeiro Teste

1. Coloque o `LevelManager` e o `GameManager` na Cena (`SampleScene`).
2. Garanta que todas as referências dos ScriptableObjects foram preenchidas.
3. Aperte o botão **Play** no editor.
4. Clique com o botão esquerdo no topo da tela. A primeira bola será instanciada e vai iniciar a queda pela física da Unity!
