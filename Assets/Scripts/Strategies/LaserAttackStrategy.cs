using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Strategies
{
    /// <summary>
    /// Instant-hit laser attack.
    /// </summary>
    public class LaserAttackStrategy : IAttackStrategy
    {
        public void Attack(Enemy target, Transform firePoint, TowerConfig config)
        {
            if (target == null) return;

            // Visuals would go here (e.g., LineRenderer)
            // Debug.Log($"Laser fired at {target.name} for {config.damage} damage");

            target.TakeDamage(config.damage);
        }
    }
}
