# Instruções de Configuração - NeonDefense

## 1. Configuração dos ScriptableObjects (Unity Editor)

O projeto utiliza **ScriptableObjects** para facilitar o balanceamento do jogo sem precisar alterar código.

### Passo A: Criar Configuração de Inimigo (`EnemyConfig`)
1. Na janela **Project**, clique com o botão direito em uma pasta (ex: `Assets/Data/Enemies`).
2. Selecione: `Create` -> `NeonDefense` -> `EnemyConfig`.
3. Nomeie o arquivo (ex: `VirusTypeA`).
4. No Inspector, configure:
   - **Enemy Name**: Nome para identificação (ex: "Basic Virus").
   - **Prefab**: Arraste o prefab do inimigo (deve conter o script `Enemy`).
   - **Health**: Vida inicial (ex: 100).
   - **Speed**: Velocidade de movimento (ex: 3).
   - **Bit Drop**: Quantidade de bits (dinheiro) ganho ao matar (ex: 15).
   - **Damage To Player**: Dano causado ao Core se chegar ao final (ex: 1).

### Passo B: Criar Configuração de Torre (`TowerConfig`)
1. Clique com o botão direito -> `Create` -> `NeonDefense` -> `TowerConfig`.
2. Configure:
   - **Tower Name**: Nome da torre.
   - **Cost**: Custo em bits.
   - **Range/FireRate/Damage**: Atributos de combate.
   - **Strategy Type**: Escolha o comportamento (`Laser` para ataque instantâneo, `Missile` para projéteis).
   - **Projectile Prefab**: **Obrigatório** se o tipo for `Missile`. Arraste o prefab do projétil (deve conter script `Projectile`).

### Passo C: Criar Configuração da Wave (`WaveConfig`)
1. Clique com o botão direito -> `Create` -> `NeonDefense` -> `WaveConfig`.
2. No Inspector, localize a lista **Enemy Groups**.
3. Adicione um novo elemento à lista:
   - **Enemy Config**: Arraste o arquivo `VirusTypeA` que você criou.
   - **Count**: Quantidade de inimigos deste tipo nesta leva (ex: 10).
   - **Spawn Rate**: Tempo de intervalo entre cada spawn (ex: 0.5s).
4. Defina **Time Between Groups** se houver mais de um grupo na mesma onda.

### Passo D: Configurar o WaveManager
1. Selecione o objeto **GameManager** (ou onde o script `WaveManager` estiver anexado).
2. Na lista **Waves**, arraste os arquivos `WaveConfig` na ordem desejada.
3. Configure os **Waypoints**:
   - Crie objetos vazios na cena representando o caminho.
   - Arraste-os para a lista **Waypoints** no Inspector. O primeiro waypoint é o ponto de spawn.
4. Marque **Auto Start** se desejar que a primeira onda comece automaticamente após 2 segundos.

---

## 2. Segredos do GitHub (DevOps)

Para que o pipeline de CI/CD (`.github/workflows/deploy.yml`) funcione corretamente e gere os builds, você precisa adicionar os seguintes **Secrets** no repositório do GitHub:

1. Vá em `Settings` -> `Secrets and variables` -> `Actions`.
2. Clique em `New repository secret`.
3. Adicione as seguintes chaves:

| Nome da Secret | Descrição | Como Obter |
| :--- | :--- | :--- |
| `UNITY_LICENSE` | Conteúdo do arquivo `.ulf` (XML). | [Ativação Manual Unity](https://license.unity3d.com/manual) ou extrair via linha de comando. Copie todo o conteúdo XML. |
| `UNITY_EMAIL` | Seu email da Unity ID. | O email usado para logar no Unity Hub. |
| `UNITY_PASSWORD` | Sua senha da Unity ID. | A senha da sua conta Unity. |

> **Nota:** O workflow usa `game-ci/unity-builder` que tentará ativar a licença usando estas variáveis de ambiente antes de compilar.

---

## 3. Como Gerar uma Release Automática

O sistema de deploy é acionado por **Tags**.

1. Finalize suas alterações e faça commit.
2. Crie uma tag de versão (deve começar com 'v'):
   ```bash
   git tag v1.0
   git push origin v1.0
   ```
3. Acompanhe a aba **Actions** no GitHub.
4. Ao final do processo (Build Windows + Build WebGL), uma **Release** será criada automaticamente contendo `Windows.zip` e `WebGL.zip`.
