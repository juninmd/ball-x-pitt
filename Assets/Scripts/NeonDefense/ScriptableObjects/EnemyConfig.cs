using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/Enemy Config", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [Tooltip("The Enemy prefab to instantiate. Must contain the Enemy component.")]
        public Enemy prefab;

        [Header("Stats")]
        [Range(1f, 10000f)]
        public float health = 100f;

        [Range(0.1f, 50f)]
        public float speed = 5f;

        [Range(1, 1000)]
        public int bitDrop = 10;

        [Range(1, 1000)]
        public int damageToPlayer = 1;
    }
}
