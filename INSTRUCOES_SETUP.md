# Instruções de Setup - Ball-x-Pitt

Bem-vindo ao projeto Ball-x-Pitt! Aqui você encontrará as instruções necessárias para configurar a física, os ScriptableObjects no Editor e habilitar o pipeline de CI/CD.

## 1. Configurando a Física (Physics Material 2D)

O jogo utiliza bastante a engine de física da Unity. Para garantir que as esferas pulem adequadamente:

1. Na aba **Project**, crie um novo Material Físico 2D (`Create > 2D > Physics Material 2D`).
2. Dê o nome de `BouncyMaterial`.
3. Selecione o `BouncyMaterial` e no **Inspector**, configure a propriedade **Bounciness** para um valor alto (ex: `0.8` ou `0.9`) para esferas mais elásticas, e ajuste o **Friction** (Geralmente `0` a `0.1` funciona bem para Pachinko).
4. Aplique este `BouncyMaterial` ao componente `Rigidbody2D` e/ou `Collider2D` no Prefab da sua **Ball** (Esfera) e também nos obstáculos do cenário.

## 2. Configurando ScriptableObjects

Os ScriptableObjects centralizam as configurações para evitar alterações dispersas no código.

### Criando BallConfig
1. No Editor, clique com o botão direito: `Create > BallXPitt > BallConfig`.
2. Dê o nome, por exemplo, `DefaultBallConfig`.
3. Configure:
   - **Mass:** `1` (ou ajuste conforme necessário).
   - **Prefab:** Arraste o seu Prefab da esfera que possui os componentes `Ball.cs`, `Rigidbody2D` e `CircleCollider2D`.
   - **Collision VFX Prefab:** Arraste o Prefab de ParticleSystem que será instanciado na colisão.
   - **Base Score:** A pontuação base (ex: 100).

### Criando LevelConfig
1. Clique com o botão direito: `Create > BallXPitt > LevelConfig`.
2. Dê o nome, por exemplo, `Level_1`.
3. Configure:
   - **Max Balls:** Quantidade de bolas permitidas para o nível (ex: 10).
   - **Score To Win:** Pontuação necessária (ex: 1000).
   - **Layout Prefab:** O Prefab do seu pit com os obstáculos posicionados.
   - **Min X / Max X:** Limites da posição horizontal de onde o jogador pode lançar a bola.
   - **Spawn Height:** A altura no eixo Y de onde a bola cairá.

## 3. GitHub Secrets para CI/CD

Para o Game-CI compilar o projeto com sucesso usando o GitHub Actions, você deve adicionar as credenciais da Unity aos Secrets do repositório no GitHub.

Vá em `Settings > Secrets and variables > Actions > New repository secret` e adicione os seguintes:

* `UNITY_LICENSE`: O conteúdo do arquivo `.ulf` da sua licença Unity gerada. (Requerido pelo Game-CI para ativar a licença offline).
* `UNITY_EMAIL`: O seu e-mail associado à conta Unity.
* `UNITY_PASSWORD`: A senha da sua conta Unity.

---
Após configurar esses itens, você estará pronto para lançar a primeira bola e criar tags (`v1.0`) para testar o sistema de automação e release automático!
