using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int CurrentTotalScore { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Keep alive between scenes if necessary
            // DontDestroyOnLoad(gameObject);
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
        }

        private void AddScore(int amount, Vector3 position)
        {
            CurrentTotalScore += amount;

            // Optional: You could trigger UI updates or floating text events here
            // e.g. GameEvents.OnScoreUIUpdate?.Invoke(CurrentTotalScore);
        }
    }
}
