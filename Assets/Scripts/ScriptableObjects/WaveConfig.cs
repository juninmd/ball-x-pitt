using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.ScriptableObjects
{
    [System.Serializable]
    public struct EnemyGroup
    {
        public EnemyConfig enemyConfig;
        public int count;
        public float spawnRate; // Time between individual enemy spawns in this group
    }

    [CreateAssetMenu(fileName = "NewWaveConfig", menuName = "NeonDefense/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [Header("Wave Configuration")]
        public List<EnemyGroup> enemyGroups;
        public float timeBetweenGroups = 2.0f;
    }
}
