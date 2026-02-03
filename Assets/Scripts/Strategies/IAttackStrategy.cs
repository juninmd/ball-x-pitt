using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Strategies
{
    public interface IAttackStrategy
    {
        void Attack(Enemy target, Transform firePoint, TowerConfig config);
    }
}
