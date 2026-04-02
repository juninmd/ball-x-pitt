using UnityEngine;

namespace NeonDefense.Core
{
    // Dummy implementation to allow ProjectilePool to compile
    public class Projectile : MonoBehaviour
    {
        public Projectile prefabReference; // Needed for returning to pool

        public void DestroyProjectile()
        {
            if (ProjectilePool.Instance != null && prefabReference != null)
            {
                ProjectilePool.Instance.ReturnToPool(this, prefabReference);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
