using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Managers;

namespace NeonDefense.Strategies
{
    public class MissileAttackStrategy : IAttackStrategy
    {
        public void Attack(Enemy target, Transform firePoint, TowerConfig config)
        {
            if (ProjectilePool.Instance == null) return;

            var projectile = ProjectilePool.Instance.Get();
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;

            projectile.Initialize(target, config.damage, (p) => ProjectilePool.Instance.ReturnToPool(p));
        }
    }
}
