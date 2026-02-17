// Verified by NeonDefense DevOps
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
            base.Awake();
        }
    }
}
