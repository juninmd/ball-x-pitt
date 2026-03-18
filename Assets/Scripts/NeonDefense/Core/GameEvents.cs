using System;
using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.Core
{
    public static partial class GameEvents
    {
        public static Action OnWaveStart;
        public static Action<int> OnWaveEnd;
        public static Action<Enemy> OnEnemyKilled;
        public static Action<Enemy, int> OnEnemyReachedGoal;
        public static Action OnGameOver;
    }
}
