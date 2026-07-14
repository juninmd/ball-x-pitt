using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public BallConfig config { get; private set; }
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(BallConfig ballConfig)
        {
            config = ballConfig;

            // Apply physics configuration
            if (rb != null && config != null)
            {
                rb.mass = config.mass;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                // Note: Bounciness should be handled by assigning a PhysicsMaterial in the Editor
                // or dynamically assigning one here, but we'll assume the material is on the Collider
                // and the user configures it per the instructions.
            }

            GameEvents.OnBallSpawned?.Invoke(this);
        }

        private void Update()
        {
            // Auto-despawn cleanup if the ball falls out of the playable area
            if (transform.position.y < -15f)
            {
                Despawn();
            }
        }

        private void OnDisable()
        {
            if (gameObject.activeSelf == false) // To handle pool recycling vs true destruction
            {
                // In pooling, disabled means "destroyed" / despawned.
                GameEvents.OnBallDestroyed?.Invoke(this);
            }
        }

        public void Despawn()
        {
            if (BallPool.Instance != null && config != null)
            {
                BallPool.Instance.ReturnToPool(this, config);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}