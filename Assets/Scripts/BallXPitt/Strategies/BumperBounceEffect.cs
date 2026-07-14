using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    // Note: This is an example of an effect that can be attached to obstacles.
    // In a real scenario, an Obstacle class might hold an IEffectStrategy.
    // For Unity's component system, we often wrap strategies in a MonoBehaviour,
    // or use them as pure C# classes instantiated by the obstacle.
    // Here we show a MonoBehaviour implementation for ease of setup in the Editor.

    [RequireComponent(typeof(Collider))]
    public class BumperBounceEffect : MonoBehaviour, IEffectStrategy
    {
        [SerializeField] private float bounceForce = 10f;
        [SerializeField] private int scoreValue = 50;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Ball>(out var ball))
            {
                ApplyEffect(ball, collision);
            }
        }

        public void ApplyEffect(Ball ball, Collision collision)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // Calculate bounce direction
                Vector3 bounceDirection = collision.contacts[0].normal;

                // Add sudden impulse
                ballRb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);

                // Trigger score event
                GameEvents.OnScoreGained?.Invoke(scoreValue, collision.contacts[0].point);
            }
        }
    }
}