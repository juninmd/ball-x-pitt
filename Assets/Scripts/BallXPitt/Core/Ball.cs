// Ball-x-Pitt System
using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Strategies;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Ball : MonoBehaviour
    {
        public Rigidbody2D Rb { get; private set; }
        public BallConfig Config { get; private set; }

        private void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(BallConfig config)
        {
            Config = config;
            if (Config != null)
            {
                Rb.mass = Config.mass;

            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (Config != null && Config.collisionVFXPrefab != null)
            {
                if (BallPool.Instance != null && collision.contactCount > 0)
                {
                    BallPool.Instance.PlayVFX(Config.collisionVFXPrefab, collision.GetContact(0).point);
                }
            }


            if (collision.gameObject.TryGetComponent<IEffectStrategy>(out var effectStrategy))
            {
                effectStrategy.ApplyEffect(this, collision);
            }


            if (Config != null)
            {
                GameEvents.OnScoreGained?.Invoke(Config.baseScore, transform.position);
            }
        }

        private void Update()
        {

            if (transform.position.y < -15f)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            if (BallPool.Instance != null && Config != null)
            {
                BallPool.Instance.ReturnToPool(this, Config);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            GameEvents.OnBallDestroyed?.Invoke(this);
        }

        private void OnEnable()
        {
            GameEvents.OnBallSpawned?.Invoke(this);
        }
    }
}
