# Instruções de Configuração - NeonDefense

## 1. Configuração dos ScriptableObjects (Unity Editor)

Para criar a primeira onda de inimigos, siga estes passos no Editor da Unity:

### Passo A: Criar Configuração de Inimigo
1. Na janela **Project**, clique com o botão direito em uma pasta (ex: `Assets/Data/Enemies`).
2. Selecione: `Create` -> `NeonDefense` -> `EnemyConfig`.
3. Nomeie o arquivo (ex: `VirusTypeA`).
4. No Inspector, configure:
   - **Prefab**: Arraste o prefab do inimigo.
   - **Health**: Vida do inimigo (ex: 100).
   - **Speed**: Velocidade (ex: 3).
   - **Bit Drop**: Dinheiro ganho ao matar (ex: 15).
   - **Damage To Player**: Dano ao Core (ex: 1).

### Passo B: Criar Configuração de Torre (Opcional)
1. Clique com o botão direito -> `Create` -> `NeonDefense` -> `TowerConfig`.
2. Configure **Range**, **FireRate**, **Damage** e escolha o **Strategy Type** (Laser, Missile, etc).

### Passo C: Criar Configuração da Wave
1. Clique com o botão direito -> `Create` -> `NeonDefense` -> `WaveConfig`.
2. No Inspector, localize a lista **Enemy Groups**.
3. Adicione um novo elemento à lista:
   - **Enemy Config**: Arraste o arquivo `VirusTypeA` que você criou.
   - **Count**: Quantidade de inimigos (ex: 10).
   - **Spawn Rate**: Tempo entre cada inimigo deste grupo (ex: 0.5).
4. Defina **Time Between Groups** se houver mais de um grupo na mesma onda.

### Passo D: Configurar o WaveManager
1. Selecione o objeto **GameManager** (ou onde o script `WaveManager` estiver).
2. Na lista **Waves**, arraste o arquivo `WaveConfig` que você criou.
3. Configure os **Waypoints** (pontos por onde os inimigos passarão).
4. Marque **Auto Start** se desejar que comece automaticamente.

---

## 2. Segredos do GitHub (DevOps)

Para que o pipeline de CI/CD (`.github/workflows/deploy.yml`) funcione, você precisa adicionar os seguintes **Secrets** no repositório do GitHub (`Settings` -> `Secrets and variables` -> `Actions`):

| Nome da Secret | Descrição |
| :--- | :--- |
| `UNITY_LICENSE` | O conteúdo do arquivo de licença da Unity (`.ulf`). *Recomendado para Unity Personal/Plus/Pro.* |
| `UNITY_EMAIL` | O email da sua conta Unity ID. |
| `UNITY_PASSWORD` | A senha da sua conta Unity ID. |

> **Nota:** Se usar apenas `UNITY_LICENSE` (arquivo de ativação manual), o email e senha podem não ser estritamente necessários dependendo da versão do `game-ci`, mas é boa prática mantê-los se o método de ativação falhar.

### Como gerar o UNITY_LICENSE:
1. Use o [Unity License Activation Tool](https://game.ci/docs/github/activation) ou rode um comando docker localmente para extrair o `.ulf`.
2. Copie todo o conteúdo do XML gerado para a secret `UNITY_LICENSE`.

---

## 3. Como Gerar uma Release

O sistema está configurado para rodar apenas quando você cria uma **Tag** de versão.

1. No terminal git:
   ```bash
   git tag v1.0
   git push origin v1.0
   ```
2. Vá para a aba **Actions** no GitHub. Você verá o workflow `Deploy` iniciar.
3. Ao final, uma nova **Release** será criada com os arquivos `Windows.zip` e `WebGL.zip` anexados.
