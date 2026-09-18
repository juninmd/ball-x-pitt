# Limitações de Testes na Sandbox

Como a Sandbox atual é um ambiente de terminal puro (sem interface gráfica e sem a engine Unity instalada ou executando), não é possível rodar a engine de física da Unity em tempo real para testes de integração.

As seguintes limitações existem durante o desenvolvimento neste ambiente:
1. **APIs de Física**: Testes interativos utilizando APIs como `Rigidbody2D`, `Collider2D` e simulação de colisões reais da Unity não podem ser executados e validados na Sandbox.
2. **Smoke Tests Lógicos**: Apenas testes de unidade de código lógico (ex: testes de matemática, estado de instâncias) que não dependam da inicialização do Unity Player/Editor podem ser feitos via ferramentas de CI/CD padrão se configurados apropriadamente.
3. **Test Runner**: Para validarmos corretamente a física (`OnCollisionEnter2D`, dinâmicas de BallPool com GameObjects reais, uso de `PhysicsMaterial2D`), o código deve ser executado no próprio Unity Editor, ou através do Unity Test Runner dentro de um ambiente provido pelo CI/CD da Unity (como o github actions utilizando as instâncias do `game-ci`).

Por enquanto, qualquer smoke test nos arquivos de C# (`Ball.cs`, `LevelManager.cs`, etc.) no ambiente Sandbox consistirá de verificação sintática estática (`python3 verify_syntax.py`) e revisão lógica das interações entre as classes. A validação empírica da física de quiques deve ser feita pelo desenvolvedor dentro do Editor da Unity.
