# Guia de Configuração: Ball-x-Pitt

Este documento contém as instruções para configurar o projeto na Unity e no GitHub para CI/CD.

## 1. Configurando a Física na Unity

Para que o jogo funcione corretamente como um Pachinko/Ball Pit, é crucial configurar os **Physics Materials 2D**.

1. Na Unity, crie uma pasta `Assets/Physics`.
2. Clique com o botão direito -> **Create > 2D > Physics Material 2D**.
3. Nomeie como `BallMaterial`.
4. Configure os valores:
   - **Friction (Atrito):** `0` (Para evitar que a bola fique presa nas paredes).
   - **Bounciness (Quique):** `0.6` a `0.8` (Depende do quão "pula-pula" você quer que o jogo seja).
5. Atribua este material ao `Collider2D` do Prefab da sua Bola (Ball).

## 2. Configurando ScriptableObjects

Os ScriptableObjects centralizam as configurações do jogo.

1. **BallConfig:**
   - No Editor, vá em `Assets/Configs/Balls` (crie a pasta se necessário).
   - Clique com o botão direito -> **Create > BallXPitt > BallConfig**.
   - Defina `Mass` (ex: 1), `Bounciness` (ex: 0.8), e arraste o Prefab da Bola para o campo `Prefab`.

2. **LevelConfig:**
   - Em `Assets/Configs/Levels`, crie um via **Create > BallXPitt > LevelConfig**.
   - Defina `Max Balls` (ex: 10) e `Score To Win` (ex: 1000).

## 3. Segredos do GitHub (GitHub Secrets)

Para que o workflow de CI/CD `.github/workflows/deploy.yml` funcione e consiga buildar o jogo via Game-CI, você DEVE configurar as credenciais da sua licença Unity no repositório.

Vá até a página do seu repositório no GitHub -> **Settings** -> **Secrets and variables** -> **Actions** -> **New repository secret**.

Adicione os seguintes Secrets:

- `UNITY_LICENSE`: O conteúdo do seu arquivo de licença `.ulf` (Unity License File). Para gerar isso, geralmente você roda a ativação manual do Game-CI ou usa uma licença Plus/Pro. Para Personal, consulte a doc do Game-CI para obter o arquivo `.ulf`.
- `UNITY_EMAIL`: O e-mail associado à sua conta Unity.
- `UNITY_PASSWORD`: A senha associada à sua conta Unity.

*Dica:* O build é engatilhado automaticamente sempre que você cria uma tag que começa com "v" (ex: `git tag v1.0` e depois `git push origin v1.0`).
