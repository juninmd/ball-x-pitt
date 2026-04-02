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

    [CreateAssetMenu(fileName = "NewTowerConfig", menuName = "NeonDefense/Tower Config", order = 2)]
    public class TowerConfig : ScriptableObject
    {
        [Header("Prefabs")]
        public GameObject prefab;
        public Projectile projectilePrefab;

        [Header("Stats")]
        [Range(1f, 50f)]
        public float range = 5f;

        [Range(0.1f, 10f)]
        public float fireRate = 1f;

        [Range(1f, 1000f)]
        public float damage = 10f;

        [Header("Economy")]
        public int cost = 100;

        [Header("Strategy")]
        public AttackStrategyType attackType;
    }
}
