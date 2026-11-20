using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int TotalScore { get; private set; }
        private float currentMultiplier = 1f;

        [Tooltip("Upper bound for the stacked multiplier, so repeated bonuses cannot overflow the score.")]
        [SerializeField, Min(1f)] private float maxMultiplier = 10f;

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
            // Widen to long so a large score cannot wrap negative; saturate at int.MaxValue
            long finalPoints = (long)System.Math.Round(points * (double)currentMultiplier);
            TotalScore = (int)System.Math.Min(int.MaxValue, TotalScore + finalPoints);
            Debug.Log($"ScoreManager: Gained {finalPoints} points. Total Score: {TotalScore}");

        }

        public void ApplyMultiplier(float multiplier)
        {
            // NaN, zero or negative values would wipe or invert the score
            if (float.IsNaN(multiplier) || multiplier <= 0f) return;
            currentMultiplier = Mathf.Min(currentMultiplier * multiplier, maxMultiplier);
            Debug.Log($"ScoreManager: Multiplier applied. Current Multiplier: {currentMultiplier}");
        }
    }
}
