// Manages Game Round State
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;
using BallXPitt.Factory;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public LevelConfig currentLevelConfig;
        public BallConfig defaultBallConfig; // Added to supply a config on click

        private int activeBalls = 0;
        public int BallsRemaining { get; private set; }
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
        }

        private void OnDisable()
        {
            GameEvents.OnBallSpawned -= HandleBallSpawned;
            GameEvents.OnBallDestroyed -= HandleBallDestroyed;
        }

        public void StartLevel(LevelConfig config)
        {
            currentLevelConfig = config;
            BallsRemaining = config.maxBalls;
            activeBalls = 0;
            isLevelActive = true;
            GameEvents.OnLevelStarted?.Invoke(0); // Pass level index if needed

            if (config.layoutPrefab != null)
            {
                Instantiate(config.layoutPrefab, Vector3.zero, Quaternion.identity);
            }
        }

        private void HandleBallSpawned(Ball ball)
        {
            if (!isLevelActive) return;
            activeBalls++;
            BallsRemaining--;
        }

        private void HandleBallDestroyed(Ball ball)
        {
            if (!isLevelActive) return;
            activeBalls--;
            CheckLevelConditions();
        }

        private void Update()
        {
            if (!isLevelActive) return;

            if (ScoreManager.Instance != null && ScoreManager.Instance.TotalScore >= currentLevelConfig.scoreToWin)
            {
                CompleteLevel();
            }
        }

        private void CheckLevelConditions()
        {
            if (!isLevelActive) return;

            // Check if level is completed (Score reached)
            if (ScoreManager.Instance != null && ScoreManager.Instance.TotalScore >= currentLevelConfig.scoreToWin)
            {
                CompleteLevel();
            }
            // Check if game over (No balls left and no active balls)
            else if (BallsRemaining <= 0 && activeBalls <= 0)
            {
                isLevelActive = false;
                GameEvents.OnGameOver?.Invoke();
            }
        }

        private void CompleteLevel()
        {
            isLevelActive = false;
            GameEvents.OnLevelCompleted?.Invoke();
        }

        public void TrySpawnBall(float xPosition, BallConfig ballConfig)
        {
            if (!isLevelActive || BallsRemaining <= 0) return;

            xPosition = Mathf.Clamp(xPosition, currentLevelConfig.minX, currentLevelConfig.maxX);
            Vector3 spawnPosition = new Vector3(xPosition, currentLevelConfig.spawnHeight, 0);

            BallFactory.Instance.CreateBall(ballConfig, spawnPosition);
        }
    }
}
