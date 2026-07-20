using UnityEngine;
using BallXPitt.ScriptableObjects;
using BallXPitt.Managers;

namespace BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Ball : MonoBehaviour
    {
        public BallConfig Config { get; private set; }
        private Rigidbody rb;
        private SphereCollider coll;

        private const float DESPAWN_Y = -15f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            coll = GetComponent<SphereCollider>();
        }

        public void Initialize(BallConfig config)
        {
            Config = config;

            if (config != null)
            {
                rb.mass = config.mass;
                if (coll.material != null)
                {
                    coll.material.bounciness = config.bounciness;
                }
                else
                {
                    PhysicMaterial mat = new PhysicMaterial();
                    mat.bounciness = config.bounciness;
                    mat.bounceCombine = PhysicMaterialCombine.Maximum;
                    coll.material = mat;
                }
            }

            // Reset physical state
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            GameEvents.OnBallSpawned?.Invoke(this);
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
            if (!gameObject.activeInHierarchy) return;

            GameEvents.OnBallDestroyed?.Invoke(this);
            BallPool.Instance.ReturnToPool(this, Config);
        }
    }
}
