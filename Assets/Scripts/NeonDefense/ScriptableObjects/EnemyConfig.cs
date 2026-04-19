using System;
using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public Enemy prefab;

        [Range(1, 10000)]
        public float health = 100f;

        [Range(0.1f, 50f)]
        public float speed = 5f;

        [Range(1, 1000)]
        public int bitDrop = 10;

        [Range(1, 1000)]
        public int damageToPlayer = 1;
    }
}
