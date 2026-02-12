# Configuração do Projeto NeonDefense

Este guia detalha como configurar os dados do jogo e o pipeline de CI/CD.

## 1. Configuração da Cena (Managers & Pools)

Para que o jogo funcione, a cena deve conter os seguintes GameObjects com seus respectivos scripts:

1.  **GameManagers**:
    - Adicione um GameObject vazio chamado `GameManagers`.
    - Adicione os scripts: `GameManager`, `WaveManager`, `EconomyManager`.
    - No `WaveManager`, configure a lista de **Waves** e **Waypoints**.

2.  **Pools**:
    - Adicione um GameObject vazio chamado `Pools`.
    - Crie dois filhos vazios: `EnemyPool` e `ProjectilePool`.
    - No GameObject `EnemyPool`, adicione o script `EnemyPool`.
    - No GameObject `ProjectilePool`, adicione o script `ProjectilePool`.
    - **Importante:** Arraste o Prefab padrão para o campo `Prefab` do script de Pool, caso deseje um comportamento padrão.

## 2. Configuração dos ScriptableObjects (Dados do Jogo)

O jogo utiliza ScriptableObjects para definir Inimigos, Torres e Ondas. Siga os passos abaixo para criar sua primeira onda.

### Passo 1: Criar Configuração de Inimigo (`EnemyConfig`)
1.  No Unity, clique com o botão direito na janela **Project** (ex: pasta `Assets/Data/Enemies`).
2.  Vá em **Create -> NeonDefense -> EnemyConfig**.
3.  Nomeie o arquivo (ex: `Virus_Basic`).
4.  No Inspector, defina:
    -   **Enemy Name**: `Virus Alpha`
    -   **Prefab**: Arraste o Prefab do inimigo. **Importante:** O Prefab deve conter o componente `Enemy`.
    -   **Health**: `100`
    -   **Speed**: `3.5`
    -   **Bit Drop**: `15`
    -   **Damage To Player**: `1`

### Passo 2: Criar Configuração de Torre (`TowerConfig`)
1.  No Unity, clique com o botão direito na janela **Project** (ex: pasta `Assets/Data/Towers`).
2.  Vá em **Create -> NeonDefense -> TowerConfig**.
3.  Nomeie o arquivo (ex: `Tower_Missile`).
4.  No Inspector, defina:
    -   **Tower Name**: `Missile Turret`
    -   **Strategy Type**: Selecione `Missile`.
    -   **Projectile Prefab**: Arraste o Prefab do projétil. **Importante:** O Prefab deve conter o componente `Projectile`.
    -   **Range**: `10`
    -   **Fire Rate**: `1` (tiros por segundo).
    -   **Damage**: `50`

### Passo 3: Criar Configuração de Onda (`WaveConfig`)
1.  Clique com o botão direito na janela **Project** (ex: pasta `Assets/Data/Waves`).
2.  Vá em **Create -> NeonDefense -> WaveConfig**.
3.  Nomeie o arquivo (ex: `Wave_01`).
4.  No Inspector:
    -   Expanda a lista **Enemy Groups**.
    -   Clique em **+** para adicionar um grupo.
    -   **Enemy Config**: Arraste o `Virus_Basic` criado anteriormente.
    -   **Count**: `10` (quantidade de inimigos).
    -   **Spawn Rate**: `0.5` (tempo em segundos entre cada spawn).
    -   **Time Between Groups**: `5` (tempo de espera após este grupo terminar).

## 3. Configuração de DevOps (GitHub Secrets)

Para que o pipeline de CI/CD funcione e gere builds automaticamente, você deve adicionar os seguintes segredos no repositório GitHub.

Vá em **Settings -> Secrets and variables -> Actions** e adicione:

| Nome do Secret | Descrição |
| :--- | :--- |
| `UNITY_LICENSE` | O conteúdo XML do seu arquivo de licença Unity (`.ulf`). Necessário para ativar o Unity no servidor de build. |
| `UNITY_EMAIL` | O endereço de e-mail da sua conta Unity ID. |
| `UNITY_PASSWORD` | A senha da sua conta Unity ID. |

**Como obter o `UNITY_LICENSE`:**
A maneira mais fácil é usar a ferramenta de ativação do `game-ci` ou extrair de uma instalação local ativada manualmente (arquivo `C:\ProgramData\Unity\Unity_lic.ulf` no Windows ou `/Library/Application Support/Unity/Unity_lic.ulf` no Mac).
