using UnityEngine;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        public Enemies.Enemy prefab;

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