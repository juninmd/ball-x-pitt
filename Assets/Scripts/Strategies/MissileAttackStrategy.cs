using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Managers;
using NeonDefense.Towers;
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
            if (ProjectilePool.Instance != null && config.projectilePrefab != null)
            {
                projectile = ProjectilePool.Instance.Get(config.projectilePrefab);
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = firePoint.rotation;
            }
            else if (config.projectilePrefab != null)
            {
                // Fallback instantiation
                Projectile obj = Object.Instantiate(config.projectilePrefab, firePoint.position, firePoint.rotation);
                projectile = obj;
            }

            if (projectile != null)
            {
                projectile.Initialize(target, config.damage);
            }
        }
    }
}
