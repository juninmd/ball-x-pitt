using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private int maxBalls = 10;
        [SerializeField] private int targetScore = 5000;

        [Header("Ball Spawning")]
        [SerializeField] private BallConfig defaultBallConfig;
        [SerializeField] private Transform spawnPoint;

        private int ballsUsed = 0;
        private int currentScore = 0;
        private int activeBallsCount = 0;

        private void OnEnable()
        {
            GameEvents.OnBallSpawned += HandleBallSpawned;
            GameEvents.OnBallDespawned += HandleBallDespawned;
            GameEvents.OnTargetHit += HandleTargetHit;
        }

        private void OnDisable()
        {
            GameEvents.OnBallSpawned -= HandleBallSpawned;
            GameEvents.OnBallDespawned -= HandleBallDespawned;
            GameEvents.OnTargetHit -= HandleTargetHit;
        }

        private void Start()
        {
            // Pre-allocate some balls to prevent GC spikes
            if (BallPool.Instance != null && defaultBallConfig != null)
            {
                BallPool.Instance.PreAllocate(defaultBallConfig, maxBalls);
            }
        }

        private void Update()
        {
            // Example input: Click to spawn ball
            if (Input.GetMouseButtonDown(0))
            {
                SpawnBall();
            }
        }

        public void SpawnBall()
        {
            if (ballsUsed < maxBalls)
            {
                Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : new Vector3(0, 5, 0);

                // Emulate selecting horizontal position based on mouse X (simple representation)
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    spawnPos.x = hit.point.x;
                }

                BallPool.Instance.GetBall(defaultBallConfig, spawnPos, Quaternion.identity);
                ballsUsed++;
            }
            else
            {
                Debug.Log("No balls left!");
            }
        }

        private void HandleBallSpawned(Ball ball)
        {
            activeBallsCount++;
        }

        private void HandleBallDespawned(Ball ball)
        {
            activeBallsCount--;
            CheckLevelState();
        }

        private void HandleTargetHit(Ball ball, int score)
        {
            currentScore += score;
            Debug.Log($"Target Hit! Score +{score}. Total Score: {currentScore}");
            CheckLevelState();
        }

        private void CheckLevelState()
        {
            if (currentScore >= targetScore)
            {
                Debug.Log("Level Complete!");
                GameEvents.OnLevelComplete?.Invoke();
            }
            else if (ballsUsed >= maxBalls && activeBallsCount == 0)
            {
                Debug.Log("Game Over!");
                GameEvents.OnGameOver?.Invoke();
            }
        }
    }
}
