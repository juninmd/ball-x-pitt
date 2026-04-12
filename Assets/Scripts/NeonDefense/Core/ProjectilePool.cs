using UnityEngine;

namespace NeonDefense.Core
{
    /// <summary>
    /// Singleton Object Pool for projectiles to prevent memory allocations (Zero GC) during gameplay.
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
