# Instruções de Configuração: Ball-x-Pitt

Este documento contém o guia prático para testar a primeira queda de bola no Editor Unity e configurar o pipeline de DevOps com CI/CD.

## 1. Configurando a Física Básica no Unity Editor

Para garantir que a física de colisão funcione conforme o planejado, siga os passos abaixo para configurar o **Physics Material 2D**:

1.  **Crie um Physics Material 2D:**
    *   No painel *Project*, clique com o botão direito e navegue até **Create > 2D > Physics Material 2D**.
    *   Nomeie como `BouncyMaterial` (ou algo semelhante).
2.  **Ajuste as Propriedades:**
    *   Selecione o material recém-criado. No painel *Inspector*, defina:
        *   **Friction:** `0.0` a `0.2` (para que a bola não perca muita velocidade ao raspar nas paredes ou obstáculos).
        *   **Bounciness:** `0.6` a `0.9` (este é o fator de 'pulo'. 1.0 é energia conservada perfeitamente).
3.  **Crie o ScriptableObject (BallConfig):**
    *   No painel *Project*, clique com o botão direito e vá em **Create > BallXPitt > Ball Config**.
    *   Nomeie como `DefaultBall`.
    *   Selecione o arquivo gerado e, no *Inspector*, preencha:
        *   **Mass:** `1`
        *   **Bounciness:** (Pode ignorar se o `Physics Material 2D` já gerenciar isso, mas deixe como `0.8` para consistência caso seu código faça override manual no Rigidbody).
        *   **Physics Material:** Arraste o `BouncyMaterial` recém-criado para este campo.
        *   **Prefab:** Arraste o prefab visual do seu objeto bola.
4.  **Associe no Gerenciador:**
    *   Vá ao GameObject onde o `LevelManager` está anexado.
    *   Arraste o ScriptableObject `DefaultBall` para o campo *Default Ball Config* no inspetor do LevelManager.
    *   *Nota: O `LevelConfig` (layout, maxBalls) também deve ser instanciado da mesma forma através de `Create > BallXPitt > Level Config` e referenciado.*
5.  **Teste a Primeira Queda:**
    *   Ao dar Play no jogo, garanta que a câmera possua uma perspectiva correta.
    *   Dê um clique do mouse na parte superior da tela. O `LevelManager` instanciará a bola via `BallPool` usando as configurações corretas, e a gravidade fará a bola cair.

## 2. Configurando Segredos (Secrets) do GitHub para CI/CD

Para o GameCI compilar o projeto para Windows 64-bit e WebGL (conforme configurado em `.github/workflows/deploy.yml`), os seguintes Secrets precisam estar configurados em seu repositório no GitHub.

Vá até a aba **Settings > Secrets and variables > Actions > New repository secret** do seu repositório e adicione EXATAMENTE estes 3 nomes:

*   **`UNITY_LICENSE`**:
    *   Contém a string XML da sua licença ativada (alf-file convertido em ulf). Consulte a documentação oficial do GameCI se precisar de ajuda para gerá-la (você pode gerar um `.alf` localmente, subir na página de licenças da Unity e gerar o `.ulf`).
*   **`UNITY_EMAIL`**:
    *   O email associado à conta Unity que ativou a licença acima.
*   **`UNITY_PASSWORD`**:
    *   A senha associada a essa mesma conta.

O deploy.yml já está configurado para ser disparado sempre que você subir uma tag que comece com 'v' (ex: `v1.0.0`). O processo será:
1. Build para Windows e WebGL.
2. Compactação dos diretórios resultantes em .zip.
3. Criação de uma Release no repositório com changelog gerado automaticamente.
