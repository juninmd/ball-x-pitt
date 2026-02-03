using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Strategies
{
    public class LaserAttackStrategy : IAttackStrategy
    {
        public void Attack(Enemy target, Transform firePoint, TowerConfig config)
        {
            if (target != null)
            {
                // Simple instant damage
                target.TakeDamage(config.damage);

                // Visuals would go here (e.g. enabling a LineRenderer component)
                // For this scope, we just apply logic.
            }
        }
    }
}
