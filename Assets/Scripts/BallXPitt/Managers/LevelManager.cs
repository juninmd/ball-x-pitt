using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public LevelConfig currentLevelConfig;

        public int ballsRemaining { get; private set; }
        public int activeBalls { get; private set; }
        public int currentScore { get; private set; }

        private bool isLevelActive = false;

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
            GameEvents.OnBallDestroyed += HandleBallDestroyed;
            GameEvents.OnScoreGained += HandleScoreGained;
        }

        private void OnDisable()
        {
            GameEvents.OnBallSpawned -= HandleBallSpawned;
            GameEvents.OnBallDestroyed -= HandleBallDestroyed;
            GameEvents.OnScoreGained -= HandleScoreGained;
        }

        public void StartLevel(LevelConfig config)
        {
            currentLevelConfig = config;
            ballsRemaining = config.maxBalls;
            activeBalls = 0;
            currentScore = 0;
            isLevelActive = true;

            GameEvents.OnLevelStarted?.Invoke(1); // Assuming level 1 for now
        }

        // Simulates Player Input
        public void TrySpawnBall(BallConfig ballConfig, float xPosition)
        {
            if (!isLevelActive || ballsRemaining <= 0) return;

            Vector3 spawnPos = new Vector3(xPosition, 10f, 0f); // Spawns at top

            Ball spawnedBall = BallPool.Instance.GetBall(ballConfig, spawnPos, Quaternion.identity);
            spawnedBall.Initialize(ballConfig);

            ballsRemaining--;
        }

        private void HandleBallSpawned(Ball ball)
        {
            activeBalls++;
        }

        private void HandleBallDestroyed(Ball ball)
        {
            activeBalls--;
            CheckLevelCompletion();
        }

        private void HandleScoreGained(int amount, Vector3 position)
        {
            if (!isLevelActive) return;

            currentScore += amount;
            CheckLevelCompletion();
        }

        private void CheckLevelCompletion()
        {
            if (!isLevelActive) return;

            if (currentScore >= currentLevelConfig.scoreToWin)
            {
                // Win condition
                isLevelActive = false;
                GameEvents.OnLevelCompleted?.Invoke();
            }
            else if (ballsRemaining <= 0 && activeBalls <= 0)
            {
                // Lose condition
                isLevelActive = false;
                GameEvents.OnGameOver?.Invoke();
            }
        }
    }
}
