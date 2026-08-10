using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Managers;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Ball : MonoBehaviour
    {
        public BallConfig config { get; private set; }

        private Rigidbody2D rb;
        private Collider2D col;
        private const float DESPAWN_Y = -15f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        public void Initialize(BallConfig ballConfig)
        {
            this.config = ballConfig;

            if (config != null)
            {
                rb.mass = config.mass;
                col.sharedMaterial = config.physicsMaterial;
            }

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        private void Update()
        {
            if (transform.position.y < DESPAWN_Y)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            if (config != null && BallPool.Instance != null)
            {
                BallPool.Instance.ReturnToPool(this, config);
                GameEvents.OnBallDestroyed?.Invoke(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
