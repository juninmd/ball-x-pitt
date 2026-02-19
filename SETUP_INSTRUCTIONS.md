# Instruções de Configuração do NeonDefense

## 1. Configurando a Primeira Onda (ScriptableObjects)

Siga estes passos no Editor da Unity para criar e configurar sua primeira onda de inimigos:

### Passo A: Criar Configuração do Inimigo (EnemyConfig)
1. Na janela **Project**, navegue para `Assets/Scripts/ScriptableObjects/`.
2. Clique com o botão direito e selecione **Create -> NeonDefense -> EnemyConfig**.
3. Nomeie o arquivo como `BasicVirus`.
4. No Inspector, configure:
   - **Enemy Name**: "Virus V1"
   - **Prefab**: Arraste seu prefab de Inimigo (deve ter o script `Enemy` anexado).
   - **Health**: 10
   - **Speed**: 3
   - **Bit Drop**: 5 (Dinheiro ganho ao matar)
   - **Damage To Player**: 1 (Dano ao Core)

### Passo B: Criar Configuração da Onda (WaveConfig)
1. Clique com o botão direito e selecione **Create -> NeonDefense -> WaveConfig**.
2. Nomeie o arquivo como `Wave_01`.
3. No Inspector, configure:
   - **Time Between Groups**: 2 (Segundos entre grupos de inimigos).
   - **Enemy Groups**: Clique no "+" para adicionar um grupo.
     - **Enemy Config**: Arraste o `BasicVirus` criado acima.
     - **Count**: 5 (Quantidade de inimigos).
     - **Spawn Rate**: 1 (Segundos entre cada spawn).

### Passo C: Configurar o WaveManager
1. Selecione o GameObject `WaveManager` na sua cena.
2. Localize o componente `WaveManager`.
3. **Waves**: Defina o tamanho da lista como 1 e arraste o `Wave_01` para o slot.
4. **Waypoints**: Arraste os Transforms dos seus waypoints da cena para esta lista, na ordem correta (Start -> End).
5. **Auto Start**: Marque esta opção se quiser que a onda comece automaticamente ao dar Play.

---

## 2. Segredos do GitHub (DevOps)

Para habilitar o pipeline de CI/CD automatizado, adicione os seguintes Segredos nas configurações do seu repositório (`Settings -> Secrets and variables -> Actions`):

| Nome do Segredo | Descrição |
| :--- | :--- |
| `UNITY_LICENSE` | **Recomendado.** O conteúdo do seu arquivo de licença `.ulf`. <br>Isso garante maior estabilidade no build. Se fornecido, o builder o usará. |
| `UNITY_EMAIL` | O endereço de e-mail associado à sua Unity ID. |
| `UNITY_PASSWORD` | A senha da sua Unity ID. |

> **Nota:** Se você não fornecer o `UNITY_LICENSE`, o sistema tentará ativar via Email/Senha, o que pode falhar se houver autenticação de dois fatores (2FA) ou limitações de licença. A licença via arquivo (`.ulf`) é a prática recomendada para DevOps profissional.
