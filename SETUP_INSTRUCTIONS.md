# Instruções de Configuração - NeonDefense

## 1. Configurando ScriptableObjects (Dados de Design)

O jogo utiliza ScriptableObjects para gerenciar dados de Inimigos, Torres e Ondas. Isso permite que designers ajustem o balanceamento sem modificar o código.

### Criar Configurações de Inimigo (EnemyConfig)
1.  Na janela de Projeto da Unity, navegue até `Assets/Scripts/ScriptableObjects/` (ou qualquer pasta de sua preferência).
2.  Clique com o botão direito -> **Create** -> **NeonDefense** -> **EnemyConfig**.
3.  Nomeie o arquivo (ex: `FastVirus`, `TankVirus`).
4.  No Inspector, defina os valores:
    *   **Health:** Vida do inimigo (ex: 50).
    *   **Speed:** Velocidade de movimento (ex: 5).
    *   **Bit Drop:** Recompensa em Bits ao morrer (ex: 10).
    *   **Damage To Player:** Dano causado ao atingir o objetivo (ex: 1).
    *   **Prefab:** Arraste o Prefab do seu Inimigo aqui.

### Criar Configurações de Torre (TowerConfig)
1.  Clique com o botão direito -> **Create** -> **NeonDefense** -> **TowerConfig**.
2.  Nomeie o arquivo (ex: `LaserTower`, `MissileTower`).
3.  No Inspector, defina os valores:
    *   **Range:** Alcance do ataque (ex: 10).
    *   **Fire Rate:** Ataques por segundo (ex: 2).
    *   **Strategy Type:** Selecione o tipo de ataque (`Laser`, `Missile`, etc.).
    *   **Projectile Prefab:** Obrigatório se usar a estratégia `Missile`.

### Criar Configurações de Onda (WaveConfig)
1.  Clique com o botão direito -> **Create** -> **NeonDefense** -> **WaveConfig**.
2.  Nomeie o arquivo (ex: `Wave1`, `Wave2`).
3.  No Inspector, adicione elementos à lista **Enemy Groups**:
    *   **Enemy Config:** Arraste um `EnemyConfig` que você criou.
    *   **Count:** Quantidade de inimigos deste tipo.
    *   **Spawn Rate:** Tempo de espera entre cada spawn.
4.  Defina **Time Between Groups** (atraso antes do próximo grupo começar).

### Atribuindo à Cena
1.  Selecione o GameObject **WaveManager** na sua cena.
2.  Localize a lista `Waves` no Inspector.
3.  Arraste e solte seus assets de `WaveConfig` nesta lista, na ordem em que deseja que apareçam.
4.  Certifique-se de que a lista `Waypoints` também esteja preenchida com os transforms do caminho.

---

## 2. Configuração do GitHub Actions (CI/CD)

Para habilitar builds e releases automatizados, você deve configurar **Secrets** nas configurações do seu repositório GitHub.

### Secrets Necessários
Vá para **Settings** -> **Secrets and variables** -> **Actions** -> **New repository secret**.

Adicione os seguintes segredos:

1.  **`UNITY_LICENSE`**
    *   **Descrição:** O conteúdo do seu arquivo de Licença da Unity (`.ulf`).
    *   **Como obter:**
        *   Localize seu arquivo `.ulf` na sua máquina local (ex: `C:\ProgramData\Unity\Unity_v6.x.x.ulf` ou `~/.local/share/unity3d/Unity/Unity_v6.x.x.ulf`).
        *   Abra com um editor de texto.
        *   Copie todo o conteúdo XML.
        *   Cole no valor do secret.

2.  **`UNITY_EMAIL`** *(Recomendado)*
    *   **Descrição:** O endereço de e-mail associado ao seu Unity ID.

3.  **`UNITY_PASSWORD`** *(Recomendado)*
    *   **Descrição:** A senha do seu Unity ID.

### Disparando um Build
O workflow está configurado para rodar **APENAS** quando você criar uma tag começando com `v`.

1.  Faça o commit das suas alterações.
2.  Crie uma tag: `git tag v1.0`
3.  Envie a tag: `git push origin v1.0`

O GitHub Actions irá automaticamente:
1.  Compilar para Windows (64-bit) e WebGL.
2.  Criar uma Release no GitHub.
3.  Fazer upload dos builds zipados como artefatos (`Windows.zip` e `WebGL.zip`).
