# NeonDefense - Instruções de Configuração

Este documento guia você na configuração inicial do projeto e do pipeline de CI/CD.

## 1. Configuração dos ScriptableObjects (Unity Editor)

Para criar a primeira onda de inimigos, siga estes passos no Unity Editor:

### A. Criar Configuração de Inimigo (EnemyConfig)
1.  Na janela `Project`, clique com o botão direito na pasta `Assets/Data/Enemies` (crie se não existir).
2.  Selecione **Create -> NeonDefense -> EnemyConfig**.
3.  Nomeie o arquivo (ex: `BasicVirus`).
4.  No `Inspector`, configure:
    *   **Enemy Name:** "Basic Virus"
    *   **Prefab:** Arraste o prefab do seu inimigo (deve ter o script `Enemy`).
    *   **Health:** 20
    *   **Speed:** 3
    *   **Bit Drop:** 5
    *   **Damage To Player:** 1

### B. Criar Configuração de Torre (TowerConfig)
1.  Clique com o botão direito na pasta `Assets/Data/Towers`.
2.  Selecione **Create -> NeonDefense -> TowerConfig**.
3.  Nomeie o arquivo (ex: `LaserTower`).
4.  No `Inspector`, configure:
    *   **Tower Name:** "Laser Sentinel"
    *   **Cost:** 50
    *   **Prefab:** Arraste o prefab da torre.
    *   **Range:** 10
    *   **Fire Rate:** 1
    *   **Damage:** 5
    *   **Strategy Type:** Laser

### C. Criar Configuração da Onda (WaveConfig)
1.  Clique com o botão direito na pasta `Assets/Data/Waves`.
2.  Selecione **Create -> NeonDefense -> WaveConfig**.
3.  Nomeie o arquivo (ex: `Wave01`).
4.  No `Inspector`, encontre a lista **Enemy Groups**:
    *   Clique em `+` para adicionar um grupo.
    *   **Enemy Config:** Arraste o `BasicVirus` criado acima.
    *   **Count:** 10 (número de inimigos neste grupo).
    *   **Spawn Rate:** 1.5 (segundos entre cada inimigo).
5.  **Time Between Groups:** 2 (segundos antes do próximo grupo, se houver).

### D. Configurar o WaveManager na Cena
1.  Selecione o objeto `WaveManager` na hierarquia da cena.
2.  No componente `WaveManager`:
    *   **Waves:** Adicione o `Wave01` à lista.
    *   **Waypoints:** Arraste os Transforms dos waypoints do mapa em ordem (o primeiro é o spawn, o último é o destino).
    *   **Auto Start:** Marque se quiser que comece automaticamente.

---

## 2. Configuração do GitHub Actions (CI/CD)

Para que o build automático funcione, você precisa adicionar as seguintes **Secrets** no repositório do GitHub (`Settings -> Secrets and variables -> Actions`):

| Secret Name | Descrição | Como obter |
| :--- | :--- | :--- |
| `UNITY_LICENSE` | Conteúdo do arquivo `.ulf` da licença Unity. | [Instruções GameCI](https://game.ci/docs/github/activation) |
| `UNITY_EMAIL` | Email da sua conta Unity. | Sua conta Unity ID. |
| `UNITY_PASSWORD` | Senha da sua conta Unity. | Sua conta Unity ID. |

### Como Disparar um Release
O pipeline de deploy só é ativado quando você cria uma **Tag** que começa com `v`.

Exemplo via terminal:
```bash
git push
git tag v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
git push origin v1.0.0
```
Isso iniciará o workflow que compila para Windows e WebGL e cria uma Release no GitHub.
