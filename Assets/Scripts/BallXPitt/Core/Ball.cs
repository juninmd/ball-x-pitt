using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public BallConfig config { get; private set; }
        private Rigidbody rb;
        private Collider col;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
        }

        public void Initialize(BallConfig ballConfig)
        {
            config = ballConfig;

            // Apply physics settings
            if (rb != null)
            {
                rb.mass = config.mass;
                rb.velocity = Vector3.zero; // Reset velocity
                rb.angularVelocity = Vector3.zero;
            }

            if (col != null && col.material != null)
            {
                col.material.bounciness = config.bounciness;
                col.material.bounceCombine = PhysicMaterialCombine.Maximum;
            }

            GameEvents.OnBallSpawned?.Invoke(this);
        }

        private void Update()
        {
            // Auto despawn logic if it falls out of bounds (Y < -15)
            if (transform.position.y < -15f)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            GameEvents.OnBallDestroyed?.Invoke(this);
            BallPool.Instance.ReturnToPool(this, config);
        }
    }
}
