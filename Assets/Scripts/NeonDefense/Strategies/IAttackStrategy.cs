using UnityEngine;
using NeonDefense.Core;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Strategies
{
    public interface IAttackStrategy
    {
        void Attack(Enemies.Enemy target, Transform firePoint, TowerConfig config);
    }
}
