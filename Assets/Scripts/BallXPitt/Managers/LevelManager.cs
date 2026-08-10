using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;
using System.Collections.Generic;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private LevelConfig currentLevelConfig;
        [SerializeField] private BallConfig defaultBallConfig; // Pode vir de um player config futuramente

        private int ballsRemaining;
        private int activeBalls = 0;
        private bool isLevelActive = false;
        private int currentScore = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            GameEvents.OnBallDestroyed += HandleBallDestroyed;
            GameEvents.OnScoreGained += HandleScoreGained;
        }

        private void OnDisable()
        {
            GameEvents.OnBallDestroyed -= HandleBallDestroyed;
            GameEvents.OnScoreGained -= HandleScoreGained;
        }

        private void Start()
        {
            if (currentLevelConfig != null)
            {
                StartLevel(currentLevelConfig);
            }
        }

        public void StartLevel(LevelConfig levelConfig)
        {
            currentLevelConfig = levelConfig;
            ballsRemaining = currentLevelConfig.maxBalls;
            activeBalls = 0;
            currentScore = 0;
            isLevelActive = true;

            // Opcional: pré-alocar pool para evitar spike no spawn
            if (defaultBallConfig != null && BallPool.Instance != null)
            {
                BallPool.Instance.PreAllocate(defaultBallConfig, currentLevelConfig.maxBalls);
            }

            GameEvents.OnLevelStarted?.Invoke(currentLevelConfig.levelId);
        }

        private void Update()
        {
            if (!isLevelActive) return;

            // Simple input mechanic
            if (Input.GetMouseButtonDown(0) && ballsRemaining > 0)
            {
                SpawnBallAtMousePosition();
            }
        }

        private void SpawnBallAtMousePosition()
        {
            if (defaultBallConfig == null || BallPool.Instance == null) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float spawnX = Mathf.Clamp(mousePos.x, currentLevelConfig.minX, currentLevelConfig.maxX);
            Vector3 spawnPosition = new Vector3(spawnX, currentLevelConfig.spawnHeight, 0f);

            Ball newBall = BallPool.Instance.Get(defaultBallConfig, spawnPosition, Quaternion.identity);
            if (newBall != null)
            {
                newBall.Initialize(defaultBallConfig);
                ballsRemaining--;
                activeBalls++;
                GameEvents.OnBallSpawned?.Invoke(newBall);
            }
        }

        private void HandleScoreGained(int amount, Vector3 position)
        {
            currentScore += amount;
            CheckLevelEndCondition();
        }

        private void HandleBallDestroyed(Ball ball)
        {
            activeBalls--;
            CheckLevelEndCondition();
        }

        private void CheckLevelEndCondition()
        {
            if (!isLevelActive || currentLevelConfig == null) return;

            bool isWin = currentScore >= currentLevelConfig.scoreToWin;
            bool isLoss = ballsRemaining <= 0 && activeBalls <= 0 && !isWin;

            if (isWin || isLoss)
            {
                isLevelActive = false;

                if (isWin)
                {
                    GameEvents.OnLevelCompleted?.Invoke();
                }
                else
                {
                    GameEvents.OnGameOver?.Invoke();
                }
            }
        }
    }
}
