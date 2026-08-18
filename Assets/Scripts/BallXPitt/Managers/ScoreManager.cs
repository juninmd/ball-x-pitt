using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int CurrentTotalScore { get; private set; }
        private int multiplier = 1;

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
            GameEvents.OnScoreGained += AddScore;
            GameEvents.OnLevelStarted += ResetScore;
        }

        private void OnDisable()
        {
            GameEvents.OnScoreGained -= AddScore;
            GameEvents.OnLevelStarted -= ResetScore;
        }

        private void ResetScore(int levelId)
        {
            CurrentTotalScore = 0;
            multiplier = 1;
        }

        private void AddScore(int amount, Vector3 position)
        {
            CurrentTotalScore += (amount * multiplier);
        }

        public void ApplyMultiplier(int m)
        {
            multiplier *= m;
        }
    }
}