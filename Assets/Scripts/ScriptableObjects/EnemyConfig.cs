// NeonDefense Core System - EnemyConfig
using UnityEngine;

namespace NeonDefense.ScriptableObjects
{
    /// <summary>
    /// Configuration data for an Enemy type.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("General")]
        public string enemyName;
        public GameObject prefab;

        [Header("Stats")]
        [Tooltip("Health points of the enemy.")]
        public float health;
        [Tooltip("Movement speed in units per second.")]
        public float speed;
        [Tooltip("Currency awarded to player on death.")]
        public int bitDrop;
        [Tooltip("Damage dealt to player/core upon reaching the goal.")]
        public int damageToPlayer;
    }
}
