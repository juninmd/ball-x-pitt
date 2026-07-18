using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    [RequireComponent(typeof(Collider))]
    public class DestroyBallEffect : MonoBehaviour, IEffectStrategy
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Ball>(out var ball))
            {
                ApplyEffect(ball, collision);
            }
        }

        public void ApplyEffect(Ball ball, Collision collision)
        {
            if (ball != null)
            {
                ball.Despawn();
            }
        }
    }
}
