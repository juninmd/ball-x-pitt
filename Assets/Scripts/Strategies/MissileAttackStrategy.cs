using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Core;

namespace NeonDefense.Strategies
{
    /// <summary>
    /// Projectile-based missile attack.
    /// </summary>
    public class MissileAttackStrategy : IAttackStrategy
    {
        public void Attack(Enemy target, Transform firePoint, TowerConfig config)
        {
            if (target == null || config.projectilePrefab == null) return;

            // Use pool if possible
            Projectile projectile = null;
            if (ProjectilePool.Instance != null)
            {
                projectile = ProjectilePool.Instance.Get();
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = firePoint.rotation;
            }
            else
            {
                // Fallback instantiation
                GameObject obj = Object.Instantiate(config.projectilePrefab, firePoint.position, firePoint.rotation);
                projectile = obj.GetComponent<Projectile>();
            }

            if (projectile != null)
            {
                projectile.Initialize(target, config.damage);
            }
        }
    }
}
