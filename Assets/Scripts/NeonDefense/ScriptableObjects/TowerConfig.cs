using UnityEngine;
using NeonDefense.Core;

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
        public Towers.Tower prefab;
        public Projectile projectilePrefab;
        public AttackStrategyType attackStrategyType;

        [Range(1f, 50f)]
        public float range = 5f;

        [Range(0.1f, 10f)]
        public float fireRate = 1f;

        [Range(1, 1000)]
        public int damage = 10;

        public int cost = 100;
    }
}