using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    public class DestroyBallEffect : MonoBehaviour, IEffectStrategy
    {
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
                ball.Despawn();
            }
        }
    }
}
