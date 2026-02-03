using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Core; // For ProjectilePool if needed
using NeonDefense.Managers; // For ProjectilePool if needed

namespace NeonDefense.Strategies
{
    public class MissileAttackStrategy : IAttackStrategy
    {
        public void Attack(Enemy target, Transform firePoint, TowerConfig config)
        {
            if (config.projectilePrefab == null)
            {
                Debug.LogWarning($"Missile Strategy requires a projectile prefab in config: {config.name}");
                return;
            }

            // Get projectile from pool
            if (ProjectilePool.Instance == null)
            {
                 Debug.LogError("ProjectilePool not found!");
                 return;
            }

            Projectile projectile = ProjectilePool.Instance.Get();
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;
            projectile.Initialize(target, config.damage);
        }
    }
}
