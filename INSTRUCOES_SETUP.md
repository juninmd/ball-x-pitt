# Ball-x-Pitt: Instruções de Setup e Configuração

Este documento explica como configurar o projeto básico do "Ball-x-Pitt" para testes, os ScriptableObjects e os requisitos do GitHub para a esteira de CI/CD.

## 1. Configurando a Física Básica (Physics Material) e ScriptableObjects no Unity Editor

Para garantir que a primeira bola caia e quique corretamente:

### 1.1 Criar o Physics Material 2D
1. No painel **Project** do Unity, clique com o botão direito e vá em `Create > 2D > Physics Material 2D`.
2. Nomeie como `BouncyMaterial`.
3. Selecione o material criado e no **Inspector**, configure a propriedade **Bounciness** para um valor entre `0.6` e `0.9` (dependendo do quão elástica você quer que a bola seja).
4. Aplique este material nos `Collider2D` dos obstáculos e das paredes do "Pit". Como alternativa, para as bolas, adicione no Prefab da Bola.

### 1.2 Configurar o Prefab da Bola
1. Crie um objeto 2D simples (como um Sprite circular).
2. Adicione os componentes: `Rigidbody2D`, `CircleCollider2D` e o script `Ball`.
3. Associe o `BouncyMaterial` ao `CircleCollider2D`.
4. Salve como um Prefab (ex: `BaseBallPrefab`).

### 1.3 Criar o ScriptableObject BallConfig
1. No painel **Project**, clique com o botão direito e selecione `Create > BallXPitt > BallConfig`.
2. Nomeie-o, por exemplo, como `DefaultBallConfig`.
3. No **Inspector**, preencha:
   - **Mass**: `1` (ou a massa desejada).
   - **Prefab**: Arraste o `BaseBallPrefab` que você criou acima.
   - **Collision VFX Prefab**: Opcional, um `ParticleSystem` se tiver.
   - **Base Score**: `100`.

### 1.4 Configurar o LevelManager e Testar
1. Crie um GameObject vazio na cena e chame de `Managers`.
2. Adicione o componente `LevelManager` a ele.
3. Crie um `LevelConfig` da mesma forma (`Create > BallXPitt > LevelConfig`), configurando os limites (`minX`, `maxX`, `spawnHeight`) e número de bolas.
4. Assinale o `LevelConfig` e o `DefaultBallConfig` no componente `LevelManager` no Editor.
5. Inicie o jogo. Clique com o mouse na parte superior da tela para spawnar e ver a bola cair!

---

## 2. Configurando o GitHub Actions (Deploy e Secrets)

O workflow de CI/CD `.github/workflows/deploy.yml` está preparado para buildar automaticamente para WebGL e Windows 64-bits sempre que uma Release Tag (começando com `v*`, ex: `v1.0.0`) for gerada no repositório.

### 2.1 Lista Exata de Secrets do Repositório GitHub
Para que o `game-ci` consiga gerar os builds, é mandatório ativar a licença Unity via Secrets. Vá até as configurações do seu repositório no GitHub: `Settings > Secrets and variables > Actions > New repository secret` e adicione exatamente os seguintes Secrets:

1. **`UNITY_LICENSE`**: O conteúdo do arquivo `.alf` (Unity License File) ativado. (Você precisa gerar a licença pessoal via Game-CI documentation, usando a imagem docker ou localmente, ativar no site da Unity e baixar o `.ulf`). O conteúdo texto desse `.ulf` deve ser colado aqui.
2. **`UNITY_EMAIL`**: O endereço de email usado na sua conta da Unity.
3. **`UNITY_PASSWORD`**: A senha da sua conta da Unity.

**Importante:** Vá também nas configurações de Actions (`Settings > Actions > General > Workflow permissions`) e certifique-se de que a opção **"Read and write permissions"** está marcada, para permitir que o Bot crie o GitHub Release e faça o upload dos ZIPs com os artefatos gerados.
