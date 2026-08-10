using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    public class ScoreMultiplierEffect : MonoBehaviour, IEffectStrategy
    {
        [SerializeField] private int multiplier = 2;
        [SerializeField] private int baseScoreValue = 50;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Ball>(out var ball))
            {
                ApplyEffect(ball, collision);
            }
        }

        public void ApplyEffect(Ball ball, Collision2D collision)
        {
            if (ball != null)
            {
                int scoreToGain = baseScoreValue * multiplier;
                if (ball.config != null)
                {
                    scoreToGain += (ball.config.baseScore * multiplier);
                }

                GameEvents.OnScoreGained?.Invoke(scoreToGain, transform.position);
            }
        }
    }
}
