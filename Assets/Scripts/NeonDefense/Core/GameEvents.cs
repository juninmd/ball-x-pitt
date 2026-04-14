using System;

namespace NeonDefense.Core
{
    public static class GameEvents
    {
        // Evento disparado quando o inimigo morre (ex: para dar bits ao jogador)
        public static Action<Enemies.Enemy> OnEnemyKilled;

        // Evento disparado quando o inimigo atinge o Core (passando o inimigo e o dano)
        public static Action<Enemies.Enemy, int> OnEnemyReachedGoal;

        // Evento disparado no início e fim das waves
        public static Action<int> OnWaveStart;
        public static Action<int> OnWaveEnd;

        // Evento de Game Over
        public static Action OnGameOver;
    }
}
