using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Managers;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public BallConfig config { get; private set; }
        private Rigidbody rb;
        private bool isDespawning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(BallConfig ballConfig)
        {
            config = ballConfig;
            isDespawning = false;

            if (rb != null && config != null)
            {
                rb.mass = config.mass;
                // Bounciness is typically handled via a PhysicsMaterial assigned to the Collider,
                // but setting it up manually if needed. We'll rely on PhysicsMaterial for actual bounciness.
                // Reset velocity on init
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            gameObject.SetActive(true);
            GameEvents.OnBallSpawned?.Invoke(this);
        }

        private void Update()
        {
            // Auto-despawn logic if it falls out of bounds (Y < -15f)
            if (transform.position.y < -15f && !isDespawning)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            if (isDespawning) return;
            isDespawning = true;

            GameEvents.OnBallDestroyed?.Invoke(this);

            if (BallPool.Instance != null)
            {
                BallPool.Instance.ReturnToPool(this, config);
            }
            else
            {
                gameObject.SetActive(false); // Fallback
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Simple generic collision check logic can go here.
            // In a real scenario, the Strategy Pattern effects on obstacles would handle the specific logic.
        }
    }
}
