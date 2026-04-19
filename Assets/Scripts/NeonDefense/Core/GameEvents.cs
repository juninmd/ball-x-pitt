using System;

namespace NeonDefense.Core
{
    public static class GameEvents
    {
        public static Action<Enemies.Enemy> OnEnemyKilled;
        public static Action<Enemies.Enemy, int> OnEnemyReachedGoal;
        public static Action<int> OnWaveStart;
        public static Action<int> OnWaveEnd;
        public static Action OnGameOver;
    }
}
