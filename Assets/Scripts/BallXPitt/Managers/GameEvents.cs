using System;

namespace BallXPitt.Managers
{
    public static class GameEvents
    {
        public static Action<Core.Ball> OnBallSpawned;
        public static Action<Core.Ball, int> OnTargetHit;
        public static Action<Core.Ball> OnBallDespawned;
        public static Action OnLevelComplete;
        public static Action OnGameOver;
    }
}
