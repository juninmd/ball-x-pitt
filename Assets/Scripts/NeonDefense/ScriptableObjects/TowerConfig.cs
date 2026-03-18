using UnityEngine;
using NeonDefense.Core;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTowerConfig", menuName = "NeonDefense/Tower Config", order = 2)]
    public class TowerConfig : ScriptableObject
    {
        [Header("Combat Stats")]
        [Range(1f, 50f)]
        public float range = 10f;

        [Range(0.1f, 10f)]
        public float fireRate = 1f;

        [Range(1, 1000)]
        public int damage = 10;

        [Header("Economy")]
        public int cost = 100;

        [Header("Prefabs")]
        public GameObject towerPrefab;
        public Projectile projectilePrefab;
    }
}
