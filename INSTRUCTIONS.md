# NeonDefense - Guia de Configuração e DevOps

Este documento cobre as instruções para configurar o projeto na Unity e no GitHub de acordo com os requisitos.

## 1. Configurando ScriptableObjects no Editor para a Primeira Onda

O sistema foi arquitetado de modo que os dados de "Ondas" e "Inimigos" não estão rígidos em código. Eles usam *ScriptableObjects*, o que permite a um Game Designer criar centenas de fases sem encostar no código fonte.

### Passos de criação:
1. **Crie um EnemyConfig (O Vírus)**
   - No Unity Editor, clique com o botão direito na aba *Project*.
   - Selecione `Create > NeonDefense > Enemy Config`.
   - Nomeie como `BasicVirus`.
   - No *Inspector*:
     - Arraste o prefab do seu inimigo para o campo `Prefab`.
     - Defina os atributos: `Health = 100`, `Speed = 5`, `Bit Drop = 10`, `Damage To Player = 1`.
2. **Crie o WaveConfig (A Onda)**
   - Novamente na aba *Project*, clique com o botão direito.
   - Selecione `Create > NeonDefense > Wave Config`.
   - Nomeie como `Wave_01`.
   - No *Inspector*:
     - Em `Enemy Groups`, adicione um novo elemento à lista.
     - Arraste o seu `BasicVirus` (criado acima) para o campo `Enemy Config`.
     - Defina a contagem (`Count = 10`) e a taxa de spawn (`Spawn Rate = 1` - 1 por segundo).
     - Se quiser que essa onda tenha dois tipos de vírus juntos, adicione mais um item à lista `Enemy Groups`.
3. **Configure o WaveManager na Cena**
   - Crie um GameObject vazio na cena chamado `Managers`.
   - Adicione o componente `WaveManager`.
   - Na lista `Waves` no Inspector, adicione o `Wave_01` que você acabou de criar.
   - Ative a flag `Auto Start` caso queira que inicie a onda automaticamente ao dar "Play".

## 2. Configurando Segredos do GitHub (CI/CD)

O arquivo `.github/workflows/deploy.yml` fará o build automático (WebGL e Windows 64) toda vez que uma tag `v*` (ex: `v1.0.0`) for publicada no GitHub e vai criar uma Release no repositório.

Porém, como a Unity exige uma licença válida para rodar compilação por linha de comando (`game-ci`), você precisará adicionar os seguintes **Secrets** no seu repositório do GitHub.

Para adicionar, vá em: `Settings > Secrets and variables > Actions > New repository secret`.

Adicione os três secrets exatos abaixo:

| Nome do Secret | O que colocar |
| :--- | :--- |
| `UNITY_EMAIL` | O e-mail da sua conta da Unity (ex: seuemail@gmail.com). |
| `UNITY_PASSWORD` | A senha da sua conta da Unity. |
| `UNITY_LICENSE` | O arquivo de licença `.ulf` gerado (Veja abaixo como gerar). |

**Nota sobre a Licença (`UNITY_LICENSE`):**
A versão Personal da Unity necessita que você ative uma licença temporária para o servidor CI. Para gerar isso facilmente:
1. Recomendamos seguir a documentação do `game-ci` na seção de [Activation](https://game.ci/docs/github/activation) onde mostram como extrair o seu `.ulf` no seu computador local e colar o conteúdo XML dentro desse Secret.