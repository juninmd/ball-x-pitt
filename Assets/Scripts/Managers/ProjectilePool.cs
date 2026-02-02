using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Towers;

namespace NeonDefense.Managers
{
    public class ProjectilePool : ObjectPool<Projectile>
    {
        public static ProjectilePool Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            base.Awake();
        }
    }
}
