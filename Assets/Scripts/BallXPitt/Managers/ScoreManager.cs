using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int TotalScore { get; private set; }
        public float CurrentMultiplier { get; private set; } = 1.0f;

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
            GameEvents.OnLevelStarted += ResetScore;
            GameEvents.OnScoreGained += AddScore;
        }

        private void OnDisable()
        {
            GameEvents.OnLevelStarted -= ResetScore;
            GameEvents.OnScoreGained -= AddScore;
        }

        private void ResetScore(int maxBalls)
        {
            TotalScore = 0;
            CurrentMultiplier = 1.0f;
            Debug.Log("Score Reset.");
        }

        private void AddScore(int amount, Vector3 position)
        {
            int calculatedScore = Mathf.RoundToInt(amount * CurrentMultiplier);
            TotalScore += calculatedScore;
            Debug.Log($"Scored! {amount} x {CurrentMultiplier} = {calculatedScore}. Total: {TotalScore}");
        }

        public void ApplyMultiplier(float multiplier)
        {
            CurrentMultiplier += multiplier;
            Debug.Log($"Multiplier increased to {CurrentMultiplier}");
        }

        public void ResetMultiplier()
        {
            CurrentMultiplier = 1.0f;
        }
    }
}