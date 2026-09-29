# Configuração do Projeto Ball-x-Pitt

Este guia fornece as instruções necessárias para testar o sistema de física e as configurações de CI/CD.

## 1. Configuração da Física no Unity Editor

Para ver a física básica funcionando (a primeira queda da bola):

1. **Criação do Physics Material:**
   - Na janela Project, clique com o botão direito -> `Create` -> `2D` -> `Physics Material 2D`.
   - Dê o nome de `BallPhysicsMaterial`.
   - Ajuste o `Friction` (atrito) para `0.0`.
   - Ajuste o `Bounciness` (quique) para `0.8` (ou outro valor de sua preferência para o estilo Pachinko).

2. **Configuração dos ScriptableObjects:**
   - **BallConfig:** Clique com o botão direito -> `Create` -> `BallXPitt` -> `BallConfig`.
     - Atribua um Prefab de bola (que deve ter um `Rigidbody2D` e um `CircleCollider2D`).
     - **Importante:** Arraste o `BallPhysicsMaterial` recém-criado para a propriedade `Material` do `CircleCollider2D` do Prefab.
     - Ajuste a massa para `1.0`.
   - **LevelConfig:** Clique com o botão direito -> `Create` -> `BallXPitt` -> `LevelConfig`.
     - Configure o `spawnHeight` para o topo da câmera (ex: `10`), e `minX`/`maxX` para as bordas horizontais.

3. **Iniciando o Teste:**
   - Adicione os scripts `GameManager`, `LevelManager`, `ScoreManager` e `BallPool` em GameObjects vazios na sua cena.
   - Configure as referências de `BallConfig` e `LevelConfig` no `LevelManager` e no `GameManager`.
   - Ao rodar o jogo (Play), use um script temporário de input ou modifique o input padrão para chamar `LevelManager.Instance.TrySpawnBall(x, config)`. A bola cairá obedecendo à física e usando a re-instanciação com Object Pooling.

## 2. Configuração de Secrets no GitHub (CI/CD)

Para que o workflow `.github/workflows/deploy.yml` funcione corretamente e gere os builds (Windows/WebGL), você precisará adicionar os seguintes **Secrets** no seu repositório do GitHub (em *Settings* -> *Secrets and variables* -> *Actions*):

- `UNITY_LICENSE` (O conteúdo completo do arquivo de licença `.ulf` da Unity).
- `UNITY_EMAIL` (O email da conta Unity).
- `UNITY_PASSWORD` (A senha da conta Unity).
