using System;
using UnityEngine;

namespace BallXPitt.Core
{
    public static class GameEvents
    {
        public static Action<Ball> OnBallSpawned;
        public static Action<Ball> OnBallDestroyed; // Renamed conceptually to Destroyed for logic, handles returning to pool
        public static Action<int, Vector3> OnScoreGained; // amount, position (for UI/VFX)
        public static Action<int> OnLevelStarted;
        public static Action OnLevelCompleted;
        public static Action OnGameOver;
    }
}
