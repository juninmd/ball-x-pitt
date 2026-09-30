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
            config = cfg;
            rb.mass = config.mass;

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (config != null && config.collisionVFXPrefab != null)
            {
                Vector2 contactPoint = collision.GetContact(0).point;
                BallPool.Instance.PlayVFX(config.collisionVFXPrefab, contactPoint);
            }

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
            if (transform.position.y < -15f)
            {
                Despawn();
            }
        }
    }
}
