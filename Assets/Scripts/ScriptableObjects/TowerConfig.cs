using UnityEngine;
using NeonDefense.Core;

namespace NeonDefense.ScriptableObjects
{
    /// <summary>
    /// Defines the attack behavior type for a tower.
    /// </summary>
    public enum AttackStrategyType
    {
        Laser,
        Missile,
        Slow
    }

    /// <summary>
    /// Configuration data for a Tower type.
    /// </summary>
    [CreateAssetMenu(fileName = "NewTowerConfig", menuName = "NeonDefense/TowerConfig")]
    public class TowerConfig : ScriptableObject
    {
        [Header("General")]
        public string towerName;
        public int cost;
        public GameObject prefab;
        [Tooltip("Prefab for the projectile (required for Missile strategy).")]
        public Projectile projectilePrefab;

        [Header("Combat Stats")]
        public float range;
        public float fireRate;
        public float damage;

        [Header("Behavior")]
        public AttackStrategyType strategyType;
    }
}
