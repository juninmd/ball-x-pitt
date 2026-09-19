# Instruções de Configuração - Ball-x-Pitt

Como Senior Unity Developer e Especialista em DevOps, preparei estas instruções para ajudar na configuração do seu jogo arcade "Ball-x-Pitt" (estilo Pachinko/Ball Pit).

## 1. Configuração da Física e ScriptableObjects no Unity Editor

O jogo "Ball-x-Pitt" foi projetado separando a lógica de dados da física/view através de `ScriptableObjects`, conforme as práticas de Clean Code e princípios SOLID. Siga estes passos para configurar a primeira queda de bola:

### Passo A: Criar e Configurar o Physics Material 2D
A física é essencial para os quiques das esferas nos obstáculos.
1. Na janela **Project**, clique com o botão direito na pasta `Assets` (ou crie uma pasta `Assets/Physics`).
2. Vá em `Create` -> `2D` -> `Physics Material 2D`.
3. Nomeie-o, por exemplo, como `BouncyMaterial`.
4. No **Inspector**, ajuste os valores:
   - **Friction**: `0` (para que as bolas não fiquem presas).
   - **Bounciness**: `0.8` (ou outro valor próximo de 1 para um quique elástico).

### Passo B: Criar as Configurações com ScriptableObjects
Usamos o padrão Strategy e configurações modulares.
1. **Configuração da Bola (`BallConfig`)**:
   - Vá na pasta `Assets/Data/` (crie se necessário).
   - Clique com o botão direito -> `Create` -> `BallXPitt` -> `Ball Config`.
   - Nomeie como `DefaultBall`.
   - No **Inspector**, configure:
     - **Mass**: `1`.
     - **Bounciness**: `0.8`.
     - **Physics Material**: Arraste o `BouncyMaterial` criado no Passo A.
     - **Prefab**: Arraste um Prefab de Esfera (que DEVE ter os componentes `Rigidbody2D` Dinâmico, `CircleCollider2D` e o script `Ball.cs`).
     - **Collision VFX Prefab**: Opcional, arraste um Prefab de `ParticleSystem` para os efeitos de impacto.
     - **Base Score**: `10`.

2. **Configuração do Level (`LevelConfig`)**:
   - Clique com o botão direito -> `Create` -> `BallXPitt` -> `Level Config` (assumindo que você também tenha um ScriptableObject de LevelConfig, similar ao código do `LevelManager`).
   - Configure o número máximo de bolas (`maxBalls`), limites horizontais do pit (`minX`, `maxX`), e altura do spawn (`spawnHeight`).

### Passo C: Conectar aos Managers
A arquitetura é baseada em Managers orientados a eventos (`GameManager`, `ScoreManager`, `LevelManager`) e Object Pooling (`BallPool`).
1. Crie um GameObject vazio na Cena, nomeie-o `LevelManager`.
2. Adicione o script `LevelManager.cs`.
3. No Inspector do `LevelManager`, arraste os seus ScriptableObjects recém-criados para os campos `Current Level Config` e `Default Ball Config`.
4. Certifique-se de que o objeto que contém o `BallPool.cs` também está na cena.

Ao rodar a cena e clicar no topo do "Pit" com o mouse (dentro dos limites configurados no `LevelConfig`), o Object Pooling será inicializado pelo `LevelManager.cs` através da Factory, e uma bola aparecerá caindo devido à gravidade.

---

## 2. Segredos do GitHub (DevOps) para CI/CD

O workflow de produção que criei (`.github/workflows/deploy.yml`) usa o **Game-CI** para gerar builds para **StandaloneWindows64** e **WebGL** e automaticamente criar um release zipado.

Para que a compilação nos servidores do GitHub Actions tenha sucesso e ative a licença da Unity (através da `game-ci/unity-builder`), é obrigatório configurar as seguintes **Secrets** no seu repositório:

1. Acesse o seu repositório no GitHub.
2. Navegue até: `Settings` -> `Secrets and variables` -> `Actions`.
3. Clique no botão `New repository secret`.
4. Adicione as 3 variáveis exatas abaixo:

| Nome da Secret | Descrição e Como Obter |
| :--- | :--- |
| `UNITY_LICENSE` | O conteúdo bruto do seu arquivo `.ulf` de licença Unity (em formato XML). Você pode gerar a licença localmente na sua máquina ou seguindo a [documentação do Game-CI para Ativação de Licença](https://game.ci/docs/github/activation). |
| `UNITY_EMAIL` | O endereço de email usado para fazer login na sua conta da Unity. |
| `UNITY_PASSWORD` | A senha da sua conta da Unity. |

### Fluxo de Release Automática
O CI/CD está configurado para disparar **APENAS** na criação de tags de versão.
Para gerar e lançar uma nova versão jogável:
```bash
git add .
git commit -m "Nova versão pronta para deploy"
git tag v1.0
git push origin v1.0
```
Isso irá ativar a pipeline, compilar os alvos, comprimir em ZIP e criar uma Release oficial na aba de "Releases" do GitHub!
