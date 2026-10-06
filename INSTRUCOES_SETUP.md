# Ball-x-Pitt: Instruções de Setup

Este guia rápido explica como configurar o projeto básico para rodar o "Ball-x-Pitt" (Neon Defense) e como preparar seu ambiente no GitHub.

## 1. Configurando a Física (Physics Material)

Para o jogo funcionar bem, as bolas devem quicar. Faremos isso com um `PhysicsMaterial2D`.

1. Na Unity, na aba **Project**, clique com o botão direito e selecione `Create > 2D > Physics Material 2D`.
2. Nomeie como `BouncyMaterial`.
3. Selecione o `BouncyMaterial` criado e, na aba **Inspector**, altere o **Friction** para `0` (para não travar) e o **Bounciness** para `0.8` (ou outro valor alto, indicando bastante quique).
4. Selecione o **Prefab da sua Esfera (Ball)** na pasta de Prefabs.
5. No componente **Collider 2D** (ex: `CircleCollider2D`) do prefab, arraste o `BouncyMaterial` para o campo `Material`.

## 2. Configurando os ScriptableObjects

Os ScriptableObjects guardam os dados do jogo, evitando hardcoding.

### Criando a Configuração da Bola (`BallConfig`)
1. No Unity, vá na aba **Project**, clique com o botão direito numa pasta (ex: `Assets/Scripts/BallXPitt/ScriptableObjects/Data`) e escolha `Create > BallXPitt > BallConfig`.
2. Nomeie como `DefaultBallConfig`.
3. No **Inspector**, preencha os valores:
   - **Mass**: `1` (ou o valor desejado).
   - **Prefab**: Arraste o seu prefab visual da Bola.
   - **Collision VFX Prefab**: (Opcional) Arraste um sistema de partículas para a colisão.
   - **Base Score**: `100` (pontos que ela rende).

### Criando a Configuração da Fase (`LevelConfig`)
1. Similarmente, crie o `LevelConfig` usando `Create > BallXPitt > LevelConfig` (assumindo que você tem esse menu).
2. Configure os valores:
   - **Max Balls**: O número de bolas disponíveis para o jogador.
   - **Score To Win**: A pontuação alvo para vencer a fase.
   - **Min X / Max X**: Os limites horizontais de onde a bola pode ser solta pelo mouse.
   - **Spawn Height**: A altura no eixo Y de onde as bolas vão cair.

## 3. GitHub Actions CI/CD Secrets

Para o arquivo `.github/workflows/deploy.yml` funcionar e gerar as builds automaticamente quando você criar uma Tag `v*`, é **obrigatório** configurar os seguintes Secrets no seu repositório do GitHub.

Vá em: **Settings** > **Secrets and variables** > **Actions** > **New repository secret**.

Adicione os três secrets exatamente com estes nomes:

1. `UNITY_LICENSE`: O conteúdo do seu arquivo de licença `.ulf` da Unity (necessário para o Game-CI compilar o jogo sem a interface gráfica).
2. `UNITY_EMAIL`: O e-mail associado à conta Unity que gerou a licença.
3. `UNITY_PASSWORD`: A senha da conta Unity que gerou a licença.

Com esses passos configurados, faça um commit da sua Tag `v1.0.0` e as actions criarão sua Release para Windows e WebGL!
