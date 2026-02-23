// NeonDefense - Projectile Pooling System
// Verified for robust object pooling and memory management.
using UnityEngine;

namespace NeonDefense.Core
{
    /// <summary>
    /// Singleton Object Pool specifically for Projectiles.
    /// Manages reusable projectile instances to avoid Garbage Collection (GC) spikes during intense combat.
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

            // Initialize the pool from the base class (pre-warms the pool)
            base.Awake();
        }
    }
}
