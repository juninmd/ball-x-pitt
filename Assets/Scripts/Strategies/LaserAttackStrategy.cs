using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Strategies
{
    public class LaserAttackStrategy : IAttackStrategy
    {
        public void Attack(Enemy target, Transform firePoint, TowerConfig config)
        {
            // Instant damage for Laser
            if (target != null)
            {
                target.TakeDamage(config.damage);

                // In a full implementation, we would enable a LineRenderer component on the Tower here.
                // For now, we simulate the logic.
            }
        }
    }
}
