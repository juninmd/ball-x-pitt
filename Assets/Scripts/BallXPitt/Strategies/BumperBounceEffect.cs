using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    public class BumperBounceEffect : MonoBehaviour, IEffectStrategy
    {
        [SerializeField] private float bounceForce = 10f;

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
                Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                if (rb != null && collision != null)
                {
                    // Calculate bounce direction from the collision point
                    Vector2 bounceDirection = (ball.transform.position - (Vector3)collision.GetContact(0).point).normalized;

                    // Reset velocity slightly before applying force for consistent bounce
                    rb.velocity *= 0.5f;
                    rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
