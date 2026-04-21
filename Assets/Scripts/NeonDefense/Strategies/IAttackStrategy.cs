using UnityEngine;

namespace NeonDefense.Strategies
{
    public interface IAttackStrategy
    {
        void Attack(Enemies.Enemy target, Transform firePoint, ScriptableObjects.TowerConfig config);
    }
}