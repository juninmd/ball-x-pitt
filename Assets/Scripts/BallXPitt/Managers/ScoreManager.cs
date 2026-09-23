using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int TotalScore { get; private set; }
        private float currentMultiplier = 1f;

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
            GameEvents.OnScoreGained += HandleScoreGained;
            GameEvents.OnLevelStarted += HandleLevelStarted;
        }

        private void OnDisable()
        {
            GameEvents.OnScoreGained -= HandleScoreGained;
            GameEvents.OnLevelStarted -= HandleLevelStarted;
        }

        private void HandleLevelStarted(int levelIndex)
        {
            TotalScore = 0;
            currentMultiplier = 1f;
        }

        private void HandleScoreGained(int points, Vector3 position)
        {
            int finalPoints = Mathf.RoundToInt(points * currentMultiplier);
            TotalScore += finalPoints;
            Debug.Log($"ScoreManager: Gained {finalPoints} points. Total Score: {TotalScore}");

            // Check win condition via LevelManager (in a decoupled way this could be checked by LevelManager listening to score events,
            // but since LevelManager manages the level, we let LevelManager check its own conditions).
        }

        public void ApplyMultiplier(float multiplier)
        {
            currentMultiplier *= multiplier;
            Debug.Log($"ScoreManager: Multiplier applied. Current Multiplier: {currentMultiplier}");
        }
    }
}
