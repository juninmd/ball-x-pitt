using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    public class BumperBounceEffect : MonoBehaviour, IEffectStrategy
    {
        [SerializeField] private float bounceForce = 10f;

        public void ApplyEffect(Ball ball, Collision2D collision)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate bounce direction
                Vector2 normal = collision.contacts[0].normal;
                Vector2 force = normal * bounceForce;

                // Add force
                rb.AddForce(force, ForceMode2D.Impulse);

                Debug.Log("BumperBounceEffect applied!");
            }
        }
    }
}
