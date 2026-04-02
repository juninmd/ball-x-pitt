using System;
using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    public static class GameEvents
    {
        public static Action<Enemy> OnEnemyKilled;
        public static Action<Enemy, int> OnEnemyReachedGoal;
        public static Action<int> OnWaveStart;
        public static Action<int> OnWaveEnd;
        public static Action OnGameOver;
    }
}
