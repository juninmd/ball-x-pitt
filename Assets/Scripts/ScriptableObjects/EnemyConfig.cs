using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("General")]
        [Tooltip("The name of the enemy type.")]
        public string enemyName;

        [Tooltip("The prefab to spawn for this enemy.")]
        public Enemy prefab;

        [Header("Stats")]
        [Tooltip("Health points of the enemy.")]
        [Range(1f, 10000f)]
        public float health = 10f;

        [Tooltip("Movement speed in units per second.")]
        [Range(0.1f, 50f)]
        public float speed = 5f;

        [Tooltip("Currency awarded to player on death (Bits).")]
        [Range(1, 1000)]
        public int bitDrop = 10;

        [Tooltip("Damage dealt to player/core upon reaching the goal.")]
        [Range(1, 1000)]
        public int damageToPlayer = 1;
    }
}
