using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Strategies;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class Ball : MonoBehaviour
    {
        public BallConfig config { get; private set; }
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(BallConfig cfg)
        {
            config = cfg;
            _rb.mass = config.mass;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }

        private void Update()
        {
            // Auto-despawn usando Y
            if (transform.position.y < -15f)
                Despawn();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (config.collisionVFXPrefab != null && collision.contactCount > 0)
            {
                BallPool.Instance.PlayVFX(config.collisionVFXPrefab, collision.GetContact(0).point);
            }

            // Strategy Pattern para acionar os efeitos dos obstáculos atingidos
            if (collision.gameObject.TryGetComponent<IEffectStrategy>(out var strategy))
            {
                strategy.ApplyEffect(this, collision);
            }
        }

        public void Despawn()
        {
            GameEvents.OnBallDestroyed?.Invoke(this);
            BallPool.Instance.ReturnToPool(this, config);
        }
    }
}
