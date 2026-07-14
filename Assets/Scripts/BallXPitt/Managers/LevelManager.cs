using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public LevelConfig currentLevelConfig;

        private int ballsRemaining;
        private bool isLevelActive;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            GameEvents.OnBallSpawned += HandleBallSpawned;
            GameEvents.OnScoreGained += HandleScoreGained;
        }

        private void OnDisable()
        {
            GameEvents.OnBallSpawned -= HandleBallSpawned;
            GameEvents.OnScoreGained -= HandleScoreGained;
        }

        public void StartLevel(LevelConfig config)
        {
            currentLevelConfig = config;
            ballsRemaining = config.maxBalls;
            isLevelActive = true;

            GameEvents.OnLevelStarted?.Invoke(config.maxBalls);
            Debug.Log($"Level Started: {config.levelName}. Balls: {ballsRemaining}");
        }

        private void HandleBallSpawned(Ball ball)
        {
            if (!isLevelActive) return;

            ballsRemaining--;
            Debug.Log($"Balls remaining: {ballsRemaining}");

            if (ballsRemaining <= 0)
            {
                CheckLevelEnd();
            }
        }

        private void HandleScoreGained(int score, Vector3 position)
        {
             // Handled by ScoreManager, LevelManager just checks win condition
             if (isLevelActive && ScoreManager.Instance.TotalScore >= currentLevelConfig.scoreToWin)
             {
                 isLevelActive = false;
                 GameEvents.OnLevelCompleted?.Invoke();
                 Debug.Log("Level Completed! Win Condition Met.");
             }
        }

        private void CheckLevelEnd()
        {
             // Wait for balls to settle/destroy before actually ending, but for simplicity:
             if (isLevelActive && ballsRemaining <= 0 && ScoreManager.Instance.TotalScore < currentLevelConfig.scoreToWin)
             {
                 // We might want to wait until active balls == 0, but this is a simplified version
                 isLevelActive = false;
                 GameEvents.OnGameOver?.Invoke();
                 Debug.Log("Game Over! Out of balls and didn't reach the target score.");
             }
        }

        // Factory Method pattern usage wrapper
        public void SpawnBall(BallConfig ballConfig, Vector3 position)
        {
             if (!isLevelActive || ballsRemaining <= 0) return;

             Ball newBall = BallPool.Instance.Get(ballConfig, position, Quaternion.identity);
             if (newBall != null)
             {
                 newBall.Initialize(ballConfig);
             }
             else
             {
                 Debug.LogError("Failed to spawn ball from pool. Check BallConfig and its prefab.");
             }
        }
    }
}