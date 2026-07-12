using UnityEngine;
using NeonDefense.BallXPitt.ScriptableObjects;
using System;

namespace NeonDefense.BallXPitt.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public BallConfig Config { get; private set; }
        private Rigidbody _rb;
        private Collider _collider;

        public static event Action<Ball> OnBallDestroyed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Initialize(BallConfig config)
        {
            Config = config;
            ApplyConfigToPhysics();
        }

        private void ApplyConfigToPhysics()
        {
            if (Config == null || _rb == null) return;

            _rb.mass = Config.mass;

            // Unity Physics Material configuration should be done on the Collider level.
            // If the collider has a material, we can adjust its bounciness, or we require
            // the user to set up a PhysicMaterial on the prefab's collider.
            if (_collider != null && _collider.material != null)
            {
                _collider.material.bounciness = Config.bounciness;
                // Important for high bounce scenarios:
                _collider.material.bounceCombine = PhysicMaterialCombine.Maximum;
            }
        }

        public void Deactivate()
        {
            // Reset velocity before returning to pool
            if (_rb != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }

            OnBallDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Future Strategy Pattern implementations for obstacles (Bumpers, etc)
            // Can be triggered from here or from the obstacles themselves.
        }
    }
}
