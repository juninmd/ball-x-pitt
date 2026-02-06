// NeonDefense Core System
using System;
using NeonDefense.Enemies;

namespace NeonDefense.Core
{
    public static class GameEvents
    {
        // Event triggered when an enemy is killed. Passes the Enemy instance and the bit reward.
        public static Action<Enemy, int> OnEnemyKilled;

        // Event triggered when an enemy reaches the goal (Core). Passes damage amount.
        public static Action<int> OnEnemyReachedGoal;

        // Event triggered when a wave starts. Passes wave index.
        public static Action<int> OnWaveStart;

        // Event triggered when a wave is cleared.
        public static Action OnWaveEnd;

        // Event triggered when player health reaches 0.
        public static Action OnGameOver;
    }
}
