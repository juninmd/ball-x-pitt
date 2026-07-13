using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Ball : MonoBehaviour
    {
        public BallConfig Config { get; private set; }

        private Rigidbody rb;
        private SphereCollider col;
        private PhysicMaterial physicsMaterial;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<SphereCollider>();
            physicsMaterial = new PhysicMaterial("BallMaterial");
            col.material = physicsMaterial;
        }

        public void Initialize(BallConfig config)
        {
            Config = config;
            ApplyConfig();
        }

        private void ApplyConfig()
        {
            if (Config == null) return;

            rb.mass = Config.mass;
            physicsMaterial.bounciness = Config.bounciness;

            // Adjust physics material properties for a bouncy pit feeling
            physicsMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
            physicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
            physicsMaterial.dynamicFriction = 0.1f;
            physicsMaterial.staticFriction = 0.1f;
        }

        private void OnEnable()
        {
            // Reset velocity when spawned from pool
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public void HitTarget(int multiplier = 1)
        {
            if (Config != null)
            {
                int totalScore = Config.baseScore * multiplier;
                Managers.GameEvents.OnTargetHit?.Invoke(this, totalScore);
            }
        }

        // This is an example to trigger despawn if it falls out of bounds (bottom of the pit)
        private void Update()
        {
            if (transform.position.y < -10f && gameObject.activeInHierarchy)
            {
                Managers.BallPool.Instance.ReturnBall(this);
            }
        }
    }
}
