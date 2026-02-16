// Verified by NeonDefense DevOps
// NeonDefense Core System - Projectile Pool
using UnityEngine;

namespace NeonDefense.Core
{
    /// <summary>
    /// Singleton Object Pool for Projectiles.
    /// Manages reusable projectile instances to avoid GC.
    /// </summary>
    [DisallowMultipleComponent]
    public class ProjectilePool : ObjectPool<Projectile>
    {
        public static ProjectilePool Instance { get; private set; }

        protected override void Awake()
        {
            // Ensure Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Initialize the pool from the base class
            // Base Awake usually handles initialization if 'prefab' is set in Inspector
            base.Awake();
        }
    }
}
