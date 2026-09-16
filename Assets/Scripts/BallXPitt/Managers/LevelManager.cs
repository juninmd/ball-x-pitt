using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private LevelConfig currentLevelConfig;
        [SerializeField] private BallConfig currentBallConfig;

        private int _ballsRemaining;
        private int _activeBalls;
        private int _currentScore;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
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

        public void StartLevel()
        {
            _ballsRemaining = currentLevelConfig.maxBalls;
            _activeBalls = 0;
            _currentScore = 0;

            // PreAllocate obrigatório para evitar Instantiate runtime
            BallPool.Instance.PreAllocate(currentBallConfig, currentLevelConfig.maxBalls);

            GameEvents.OnLevelStarted?.Invoke(0);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _ballsRemaining > 0)
                SpawnBall();
        }

        private void SpawnBall()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float spawnX = Mathf.Clamp(mousePos.x, currentLevelConfig.minX, currentLevelConfig.maxX);
            Vector3 spawnPos = new Vector3(spawnX, currentLevelConfig.spawnHeight, 0);

            Ball ball = BallPool.Instance.GetBall(currentBallConfig, spawnPos);
            ball.Initialize(currentBallConfig);

            _ballsRemaining--;
            _activeBalls++;
            GameEvents.OnBallSpawned?.Invoke(ball);
        }

        private void HandleBallDestroyed(Ball ball)
        {
            _activeBalls--;
            CheckWinCondition();
        }

        private void HandleScoreGained(int points, Vector3 pos)
        {
            _currentScore += points;
            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (_currentScore >= currentLevelConfig.scoreToWin)
                GameEvents.OnLevelCompleted?.Invoke();
            else if (_ballsRemaining == 0 && _activeBalls == 0)
                GameEvents.OnGameOver?.Invoke();
        }
    }
}
