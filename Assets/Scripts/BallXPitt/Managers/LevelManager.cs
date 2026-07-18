using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private LevelConfig currentLevel;
        [SerializeField] private BallConfig defaultBallConfig;
        [SerializeField] private Transform spawnPoint;

        [Header("State")]
        private int ballsRemaining;
        private int currentScore;
        private int activeBalls;

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

        private void Start()
        {
            if (currentLevel != null)
            {
                StartLevel(currentLevel);
            }
        }

        public void StartLevel(LevelConfig level)
        {
            currentLevel = level;
            ballsRemaining = level.maxBalls;
            currentScore = 0;
            activeBalls = 0;

            if (BallPool.Instance != null && defaultBallConfig != null)
            {
                BallPool.Instance.PreAllocate(defaultBallConfig, 10);
            }

            GameEvents.OnLevelStarted?.Invoke(1);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TrySpawnBall();
            }
        }

        public void TrySpawnBall()
        {
            if (ballsRemaining > 0)
            {
                Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : new Vector3(0, 10f, 0);

                // Allow simple player input on X axis using mouse
                if (Camera.main != null)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
                    float targetX = Camera.main.ScreenToWorldPoint(mousePos).x;
                    spawnPos.x = targetX;
                }

                if (BallPool.Instance != null && defaultBallConfig != null)
                {
                    Ball ball = BallPool.Instance.GetBall(defaultBallConfig, spawnPos, Quaternion.identity);
                    ball.Initialize(defaultBallConfig);
                    ballsRemaining--;
                }
            }
            else if (activeBalls == 0)
            {
                CheckLevelEnd();
            }
        }

        private void HandleBallSpawned(Ball ball)
        {
            activeBalls++;
        }

        private void HandleBallDestroyed(Ball ball)
        {
            activeBalls--;
            if (activeBalls <= 0 && ballsRemaining <= 0)
            {
                CheckLevelEnd();
            }
        }

        private void HandleScoreGained(int amount, Vector3 position)
        {
            currentScore += amount;
            if (currentScore >= currentLevel.targetScore)
            {
                GameEvents.OnLevelCompleted?.Invoke();
            }
        }

        private void CheckLevelEnd()
        {
            if (currentLevel == null) return;

            if (currentScore >= currentLevel.targetScore)
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
