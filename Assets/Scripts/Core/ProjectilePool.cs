using UnityEngine;

namespace NeonDefense.Core
{
    /// <summary>
    /// Singleton Object Pool specifically for Projectiles.
    /// Ensures global access to recycling logic for bullets, missiles, etc.
    /// </summary>
    [DisallowMultipleComponent]
    public class ProjectilePool : ObjectPool<Projectile>
    {
        public static ProjectilePool Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            base.Awake();
        }
    }
}
