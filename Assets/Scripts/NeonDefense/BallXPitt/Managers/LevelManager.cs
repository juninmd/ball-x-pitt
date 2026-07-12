using UnityEngine;
using System;
using NeonDefense.BallXPitt.Core;
using NeonDefense.BallXPitt.ScriptableObjects;

namespace NeonDefense.BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Settings")]
        public int maxBalls = 10;
        private int _ballsUsed = 0;
        private int _activeBallsInScene = 0;

        [Header("Dependencies")]
        public Transform spawnPoint; // General spawn area Y height

        // Events to broadcast level state
        public static event Action<int> OnBallSpawned;
        public static event Action OnLevelComplete;
        public static event Action OnOutOfBalls;

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
            Ball.OnBallDestroyed += HandleBallDestroyed;
        }

        private void OnDisable()
        {
            Ball.OnBallDestroyed -= HandleBallDestroyed;
        }

        public void SpawnBall(BallConfig config, float xPosition)
        {
            if (_ballsUsed >= maxBalls)
            {
                Debug.LogWarning("Maximum number of balls reached for this level!");
                return;
            }

            if (BallPool.Instance == null)
            {
                Debug.LogError("BallPool is missing in the scene!");
                return;
            }

            // Factory Method abstraction: Ask the Pool for the exact config's prefab
            Ball newBall = BallPool.Instance.Get(config.prefab);

            // Set Position based on input X and fixed Y spawn height
            Vector3 spawnPos = new Vector3(xPosition, spawnPoint != null ? spawnPoint.position.y : 10f, 0f);
            newBall.transform.position = spawnPos;

            // Initialize Data and Physics
            newBall.Initialize(config);

            _ballsUsed++;
            _activeBallsInScene++;

            OnBallSpawned?.Invoke(maxBalls - _ballsUsed); // Broadcast remaining balls

            CheckGameState();
        }

        private void HandleBallDestroyed(Ball ball)
        {
            _activeBallsInScene--;
            CheckGameState();
        }

        private void CheckGameState()
        {
            if (_ballsUsed >= maxBalls && _activeBallsInScene <= 0)
            {
                // Here we would typically check ScoreManager to see if points >= LevelConfig.scoreToWin
                // For now, trigger Level Complete or Out of Balls
                OnLevelComplete?.Invoke();
                OnOutOfBalls?.Invoke();
            }
        }
    }
}
