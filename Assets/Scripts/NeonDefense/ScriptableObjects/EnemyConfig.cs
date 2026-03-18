using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/Enemy Config", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        [Header("General Attributes")]
        [Tooltip("The prefab that will be instantiated by the pool.")]
        public Enemy prefab;

        [Header("Stats")]
        [Range(1, 10000)]
        public int health = 100;

        [Range(0.1f, 50f)]
        public float speed = 5f;

        [Range(1, 1000)]
        public int bitDrop = 10;

        [Range(1, 1000)]
        public int damageToPlayer = 1;
    }
}
