using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Strategies
{
    /// <summary>
    /// Interface for implementing different tower attack behaviors.
    /// </summary>
    public interface IAttackStrategy
    {
        /// <summary>
        /// Performs an attack on the target.
        /// </summary>
        /// <param name="target">The enemy to attack.</param>
        /// <param name="firePoint">The transform from where the attack originates.</param>
        /// <param name="config">The tower's configuration stats.</param>
        void Attack(Enemy target, Transform firePoint, TowerConfig config);
    }
}
