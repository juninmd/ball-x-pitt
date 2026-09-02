# Solução: Ball-x-Pitt

Este documento descreve a arquitetura base para o jogo "Ball-x-Pitt", conforme solicitado.

## Arquitetura (Clean Code e SOLID)

A estrutura adota o padrão de isolamento de responsabilidades:
- **Core:** Scripts fundamentais de comportamento (`Ball`, `BallPool`).
- **Managers:** Gerenciadores de estado (`LevelManager`).
- **ScriptableObjects:** Contêineres de dados puramente declarativos (`BallConfig`).

A comunicação inter-sistemas usa eventos estáticos sem forte acoplamento (via `GameEvents`).

## Padrões Utilizados

1.  **Object Pooling Zero-GC:** Implementado em `BallPool.cs`. Previne alocações em tempo de execução via `Instantiate` utilizando dicionários com filas pré-alocadas para esferas e partículas.
2.  **Factory Method:** Embutido em `LevelManager` e `BallPool`. A criação é delegada ao `BallPool.Get()`, que cuida de buscar objetos do Pool baseado no tipo de `BallConfig`.
3.  **Strategy Pattern (Limitação):** A fundação está em `Ball.cs`, onde as colisões invocam estratégias. *Limitação: As interfaces e classes concretas das estratégias (BumperBounceEffect, etc.) e o script GameEvents não foram inclusos nesse PR para manter o escopo reduzido, como solicitado.*

## Configuração do Editor e CI/CD

Consulte a resposta direta no chat para as instruções sobre como configurar o Editor Unity (Physics Material e Scriptable Objects) e os Secrets do GitHub para habilitar a pipeline do `.github/workflows/deploy.yml`.
