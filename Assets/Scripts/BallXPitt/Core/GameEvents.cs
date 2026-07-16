using System;
using UnityEngine;

namespace BallXPitt.Core
{
    public static class GameEvents
    {
        public static Action<Ball> OnBallSpawned;
        public static Action<Ball> OnBallDestroyed;
        public static Action<int, Vector3> OnScoreGained;
        public static Action<int> OnLevelStarted;
        public static Action OnLevelCompleted;
        public static Action OnGameOver;
    }
}
