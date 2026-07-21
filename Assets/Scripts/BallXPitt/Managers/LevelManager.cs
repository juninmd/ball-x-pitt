using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;
using System.Collections.Generic;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public LevelConfig currentLevelConfig;
        public BallConfig defaultBallConfig; // The ball to spawn on click

        private int ballsRemaining;
        private List<Ball> activeBalls = new List<Ball>();
        private bool isLevelActive = false;

        [Header("Spawn Settings")]
        public float spawnHeight = 10f; // Y position for spawned balls

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

        private void Start()
        {
            // For testing purposes, start level right away if config exists
            if (currentLevelConfig != null)
            {
                StartLevel(currentLevelConfig);
            }
        }

        public void StartLevel(LevelConfig config)
        {
            currentLevelConfig = config;
            ballsRemaining = config.maxBalls;
            activeBalls.Clear();
            isLevelActive = true;

            // Preallocate balls
            if (defaultBallConfig != null && BallPool.Instance != null)
            {
                BallPool.Instance.PreAllocate(defaultBallConfig, 5);
            }

            // In a real game, you might instantiate the level layout prefab here

            GameEvents.OnLevelStarted?.Invoke(config.targetScore);
        }

        private void Update()
        {
            if (!isLevelActive) return;

            // Simple input to spawn a ball at the top (mouse X position)
            if (Input.GetMouseButtonDown(0) && ballsRemaining > 0 && defaultBallConfig != null)
            {
                SpawnBallAtMouseX();
            }
        }

        private void SpawnBallAtMouseX()
        {
            // Convert mouse position to world position on X axis
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // Distance from camera
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 spawnPos = new Vector3(worldPos.x, spawnHeight, 0f);

            BallPool.Instance.Get(defaultBallConfig, spawnPos, Quaternion.identity);
            ballsRemaining--;
        }

        private void HandleBallSpawned(Ball ball)
        {
            if (!activeBalls.Contains(ball))
            {
                activeBalls.Add(ball);
            }
        }

        private void HandleBallDestroyed(Ball ball)
        {
            if (activeBalls.Contains(ball))
            {
                activeBalls.Remove(ball);
            }

            CheckLevelCompletion();
        }

        private void CheckLevelCompletion()
        {
            if (ballsRemaining <= 0 && activeBalls.Count == 0 && isLevelActive)
            {
                isLevelActive = false;
                GameEvents.OnLevelCompleted?.Invoke();
            }
        }
    }
}
