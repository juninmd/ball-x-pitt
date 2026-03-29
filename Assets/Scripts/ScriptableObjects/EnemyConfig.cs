using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.ScriptableObjects
{
    /// <summary>
    /// Configuration data for enemies, defining their stats, drops, and references.
    /// Used by EnemyFactory/WaveManager to instantiate enemies.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("General")]
        public string enemyName;
        public Enemy prefab;

        [Header("Stats")]
        [Range(1f, 10000f)]
        public float health = 10f;
        [Range(0.1f, 50f)]
        public float speed = 5f;

        [Header("Economy & Gameplay")]
        [Range(1, 1000)]
        public int bitDrop = 10;
        [Range(1, 1000)]
        public int damageToPlayer = 1;
    }
}
