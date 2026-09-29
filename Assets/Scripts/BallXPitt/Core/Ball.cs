// Generates core physical object logic
using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Pools;
using BallXPitt.Strategies;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        public BallConfig config { get; private set; }
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(BallConfig cfg)
        {
            config = ballConfig;
            rb.mass = config.mass;

            // Note: Bounciness is handled directly via a PhysicsMaterial2D assigned to the prefab's Collider2D in the Editor.
            // This avoids creating new material instances at runtime and causing GC allocations or overriding shared state globally.

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Apply VFX
            if (config != null && config.collisionVFXPrefab != null)
            {
                Vector2 contactPoint = collision.GetContact(0).point;
                BallPool.Instance.PlayVFX(config.collisionVFXPrefab, contactPoint);
            }

            // Apply strategy effects from obstacles
            if (collision.gameObject.TryGetComponent<IEffectStrategy>(out var effectStrategy))
            {
                effectStrategy.ApplyEffect(this, collision);
            }
        }

        public void Despawn()
        {
            GameEvents.OnBallDestroyed?.Invoke(this);
            if (BallPool.Instance != null && config != null)
            {
                BallPool.Instance.ReturnToPool(this, config);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            // Auto despawn logic if fallen out of bounds
            if (transform.position.y < -15f)
            {
                // Could also trigger a score zone event here if not handled by triggers
                Despawn();
            }
        }
    }
}
// Generated for BallXPitt
// Update for Ball-x-Pitt PR
