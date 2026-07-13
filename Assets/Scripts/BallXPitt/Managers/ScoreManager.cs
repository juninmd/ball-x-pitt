using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int TotalScore { get; private set; }

        private void OnEnable()
        {
            GameEvents.OnTargetHit += AddScore;
        }

        private void OnDisable()
        {
            GameEvents.OnTargetHit -= AddScore;
        }

        private void AddScore(Ball ball, int scoreAmount)
        {
            TotalScore += scoreAmount;
            // Optionally notify UI
            Debug.Log($"[ScoreManager] Score Updated: {TotalScore}");
        }
    }
}
