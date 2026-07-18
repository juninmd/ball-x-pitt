using UnityEngine;
using BallXPitt.Core;
using BallXPitt.Managers;

namespace BallXPitt.Strategies
{
    [RequireComponent(typeof(Collider))]
    public class ScoreMultiplierEffect : MonoBehaviour, IEffectStrategy
    {
        [SerializeField] private float multiplierIncrease = 0.5f;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Ball>(out var ball))
            {
                ApplyEffect(ball, collision);
            }
        }

        public void ApplyEffect(Ball ball, Collision collision)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ApplyMultiplier(multiplierIncrease);
            }
        }
    }
}
