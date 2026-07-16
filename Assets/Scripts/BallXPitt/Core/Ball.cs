using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        private Rigidbody rb;
        private Collider col;
        private PhysicMaterial pMaterial;
        public BallConfig config { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();

            if (col != null)
            {
                pMaterial = new PhysicMaterial($"{gameObject.name}_PhysicsMaterial");
                pMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
                pMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
                col.sharedMaterial = pMaterial;
            }
        }

        public void Initialize(BallConfig ballConfig)
        {
            config = ballConfig;

            if (rb != null)
            {
                rb.mass = config.mass;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (pMaterial != null)
            {
                pMaterial.bounciness = config.bounciness;
            }

            GameEvents.OnBallSpawned?.Invoke(this);
        }

        private void Update()
        {
            // Auto-despawn if it falls below -15f
            if (transform.position.y < -15f)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            GameEvents.OnBallDestroyed?.Invoke(this);

            if (config != null && BallPool.Instance != null)
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
