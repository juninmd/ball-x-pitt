using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Spawn Settings")]
        public Transform spawnPointTop;
        public float spawnAreaWidth = 5f;

        [Header("Level Data")]
        public LevelConfig currentLevelConfig;

        private int ballsRemaining = 0;
        private int activeBalls = 0;

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
            GameEvents.OnBallDestroyed += HandleBallDestroyed;
        }

        private void OnDisable()
        {
            GameEvents.OnBallDestroyed -= HandleBallDestroyed;
        }

        public void StartLevel(LevelConfig levelConfig)
        {
            currentLevelConfig = levelConfig;
            ballsRemaining = levelConfig.maxBalls;
            activeBalls = 0;
            GameEvents.OnLevelStarted?.Invoke(levelConfig.maxBalls);
        }

        public void SpawnBall(BallConfig config, float normalizedXPosition)
        {
            if (config == null || BallPool.Instance == null || spawnPointTop == null) return;
            if (ballsRemaining <= 0) return;

            // map [0, 1] to [-spawnAreaWidth/2, spawnAreaWidth/2]
            float xOffset = Mathf.Lerp(-spawnAreaWidth / 2f, spawnAreaWidth / 2f, normalizedXPosition);
            Vector3 spawnPos = spawnPointTop.position + new Vector3(xOffset, 0, 0);

            Ball newBall = BallPool.Instance.GetBall(config, spawnPos, Quaternion.identity);
            newBall.Initialize(config);

            ballsRemaining--;
            activeBalls++;
        }

        private void HandleBallDestroyed(Ball ball)
        {
            activeBalls--;

            if (ballsRemaining <= 0 && activeBalls <= 0)
            {
                GameEvents.OnLevelCompleted?.Invoke();
            }
        }
    }
}
