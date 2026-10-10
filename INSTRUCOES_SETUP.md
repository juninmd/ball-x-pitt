<!-- Projeto: Ball-x-Pitt Arcade -->
# Ball-x-Pitt: Instruções de Setup

Este documento contém as instruções para configurar o projeto na Unity e no GitHub para CI/CD, garantindo o funcionamento da física e dos builds automatizados.

## 1. Configurando a Física na Unity (Physics Material)

Para que o jogo funcione corretamente no estilo Pachinko/Ball Pit, as bolas devem quicar de forma realista. Faremos isso configurando os **Physics Materials 2D**.

1. Na Unity, crie uma pasta `Assets/Physics` (se não existir).
2. Clique com o botão direito -> **Create > 2D > Physics Material 2D**.
3. Nomeie como `BallMaterial` (ou `BouncyMaterial`).
4. Selecione o material e, no **Inspector**, configure os valores:
   - **Friction (Atrito):** `0` (Para evitar que a bola fique presa nas paredes ou perca momento).
   - **Bounciness (Quique):** `0.6` a `0.8` (Depende do quão "pula-pula" você quer que o jogo seja. 0.8 é recomendado).
5. Selecione o **Prefab da sua Esfera (Ball)** na pasta de Prefabs.
6. No componente **Collider 2D** (ex: `CircleCollider2D`) do prefab, arraste o `BallMaterial` para o campo `Material`.

## 2. Configurando os ScriptableObjects

Os ScriptableObjects centralizam as configurações do jogo, evitando hardcoding e permitindo balanceamento fácil.

### Criando a Configuração da Bola (`BallConfig`)
1. No Editor, vá em `Assets/Configs/Balls` (crie a pasta se necessário).
2. Clique com o botão direito -> **Create > BallXPitt > BallConfig**.
3. Nomeie como `DefaultBallConfig`.
4. No **Inspector**, preencha os valores:
   - **Mass**: `1` (ou ajuste conforme necessário para a física).
   - **Prefab**: Arraste o seu prefab visual da Bola.
   - **Collision VFX Prefab**: (Opcional) Arraste um sistema de partículas para a colisão.
   - **Base Score**: `100` (pontos que a bola rende ao tocar um alvo/fundo).

### Criando a Configuração da Fase (`LevelConfig`)
1. Em `Assets/Configs/Levels`, clique com o botão direito -> **Create > BallXPitt > LevelConfig**.
2. Nomeie como `Level1Config` (por exemplo).
3. Configure os valores:
   - **Max Balls**: O número de bolas disponíveis para o jogador (ex: `10`).
   - **Score To Win**: A pontuação alvo para vencer a fase (ex: `1000`).
   - **Min X / Max X**: Os limites horizontais de onde a bola pode ser solta (ex: `-5` e `5`).
   - **Spawn Height**: A altura no eixo Y de onde as bolas vão cair (ex: `10`).
   - **Layout Prefab**: Arraste um Prefab que contenha os obstáculos (Bumpers, Multiplicadores, etc) desta fase.

## 3. Segredos do GitHub (GitHub Actions CI/CD Secrets)

Para que o workflow de CI/CD `.github/workflows/deploy.yml` funcione e consiga compilar o jogo para Windows 64-bit e WebGL via Game-CI, você DEVE configurar as credenciais da sua licença Unity no repositório.

Vá até a página do seu repositório no GitHub -> **Settings** -> **Secrets and variables** -> **Actions** -> **New repository secret**.

Adicione os seguintes Secrets (exatamente com estes nomes):

- `UNITY_LICENSE`: O conteúdo do seu arquivo de licença `.ulf` (Unity License File). Para gerar isso, geralmente você roda a ativação manual do Game-CI ou usa uma licença Plus/Pro. Para Personal, consulte a doc do Game-CI para obter o arquivo `.ulf`.
- `UNITY_EMAIL`: O e-mail associado à sua conta Unity.
- `UNITY_PASSWORD`: A senha associada à sua conta Unity.

*Dica:* O workflow só roda (trigger) quando você cria uma Tag começando com "v" (ex: `v1.0.0`).
Para criar uma release com build, execute os comandos no terminal do git:
```bash
git tag v1.0.0
git push origin v1.0.0
```
Isso acionará a Action que fará o build, criará a Release no GitHub e anexará os arquivos zipados automaticamente.
