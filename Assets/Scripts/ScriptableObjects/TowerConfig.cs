using UnityEngine;

namespace NeonDefense.ScriptableObjects
{
    public enum AttackStrategyType
    {
        Laser,
        Missile,
        Slow
    }

    [CreateAssetMenu(fileName = "NewTowerConfig", menuName = "NeonDefense/TowerConfig")]
    public class TowerConfig : ScriptableObject
    {
        [Header("General")]
        public string towerName;
        public int cost;
        public GameObject prefab;
        public GameObject projectilePrefab; // For missile/projectile towers

        [Header("Combat Stats")]
        public float range;
        public float fireRate;
        public float damage;

        [Header("Behavior")]
        public AttackStrategyType strategyType;
    }
}
