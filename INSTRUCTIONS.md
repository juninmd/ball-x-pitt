# Ball-x-Pitt - Guia de Configuração e DevOps

Este documento cobre as instruções para configurar o projeto "Ball-x-Pitt" (estilo Pachinko) na Unity e configurar o pipeline de CI/CD no GitHub.

## 1. Configurando a Física e ScriptableObjects no Editor

Para testar a primeira queda de bola e garantir que a física (quique) funcione adequadamente em harmonia com o Object Pool, siga estes passos no Unity Editor:

### A. Criando os ScriptableObjects

1. **Criar a Configuração da Bola (`BallConfig`)**:
   - Na aba *Project*, clique com o botão direito e navegue até `Create > BallXPitt > Ball Config`.
   - Nomeie como `DefaultBall`.
   - No *Inspector*:
     - **Mass**: `1.0` (Ajuste conforme desejado para a gravidade e força de impacto).
     - **Bounciness**: `0.8` (Isso não aplica o bounce sozinho, você precisará do Physics Material).
     - **Prefab**: Arraste o prefab visual da sua esfera (que deve ter um `Rigidbody2D` e `Collider2D` e o script `Ball.cs` anexado).
     - **Collision VFX Prefab**: Arraste um prefab de `ParticleSystem` se tiver um, ou deixe vazio.
     - **Base Score**: `10` (pontuação ao atingir a zona de gol).

2. **Criar a Configuração da Fase (`LevelConfig`)**:
   - Na aba *Project*, clique com o botão direito e navegue até `Create > BallXPitt > Level Config`.
   - Nomeie como `Level_01`.
   - No *Inspector*:
     - **Level Id**: `1`
     - **Max Balls**: `10` (número máximo de bolas na fase).
     - **Score To Win**: `100`
     - **Spawn Height**: `10` (Altura Y no topo do Pit onde a bola vai spawnar).
     - **Min X / Max X**: `-5` e `5` (Limites laterais para o jogador selecionar onde soltar a bola).

### B. Configurando o Physics Material (Para o Bounce/Quique)

1. Na aba *Project*, clique com o botão direito e vá em `Create > 2D > Physics Material 2D`.
2. Nomeie como `BouncyMaterial`.
3. No *Inspector* deste material, configure a propriedade **Friction** para `0.0` (ou próximo disso) e **Bounciness** para `0.8` (ou o valor desejado).
4. Arraste este `BouncyMaterial` para o campo **Physics Material** no seu `DefaultBall` (BallConfig criado acima). Isso garante que, ao inicializar a bola, o collider receberá este material de quique.

### C. Configurando a Cena

1. Crie GameObjects vazios para atuar como paredes/chão e adicione `BoxCollider2D` neles para formar o "Pit" (poço).
2. Adicione os Managers em GameObjects vazios na cena (`LevelManager`, `GameManager`, `ScoreManager`, `BallPool`).
3. No componente `GameManager`, arraste o `Level_01` (LevelConfig) criado.
4. No componente `LevelManager`, certifique-se de configurar a referência de `DefaultBallConfig` com o seu `DefaultBall` recém criado.

Ao dar "Play", a lógica do jogo usará os dados dos seus ScriptableObjects. Clique na tela entre MinX e MaxX, e o `LevelManager` usará a `BallPool` para pegar uma bola e soltá-la.

## 2. Configurando Segredos do GitHub (CI/CD)

O pipeline de entrega contínua (CI/CD) usando GitHub Actions (game-ci) está configurado no arquivo `.github/workflows/deploy.yml`.

Ele compilará versões para **Windows 64-bit** e **WebGL** **apenas** quando você criar uma Tag que inicie com 'v' (ex: `v1.0.0`). Após a compilação, ele criará automaticamente uma Release no repositório do GitHub com os arquivos zipados e as release notes.

Para que a compilação funcione, você precisa fornecer as credenciais da sua licença Unity como **Secrets** no repositório.

Vá até as configurações do seu repositório no GitHub: `Settings > Secrets and variables > Actions > New repository secret`.

Adicione os **três** Secrets abaixo:

| Nome do Secret | Descrição |
| :--- | :--- |
| `UNITY_EMAIL` | O endereço de e-mail associado à sua conta Unity. |
| `UNITY_PASSWORD` | A senha da sua conta Unity. |
| `UNITY_LICENSE` | O conteúdo do arquivo de licença `.ulf`. Para contas Personal, siga a documentação do game-ci sobre [ativação](https://game-ci/docs/github/activation) para gerar este arquivo na sua máquina e cole o conteúdo de texto (XML) aqui. |

Assim que esses segredos estiverem no lugar, criar e empurrar uma tag como `v1.0` iniciará todo o processo automatizado de build e release!
