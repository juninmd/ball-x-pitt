using System;
using System.Collections.Generic;
using UnityEngine;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.ScriptableObjects
{
    [Serializable]
    public struct EnemyGroup
    {
        public EnemyConfig enemyConfig;
        public int count;
        public float spawnRate;
    }

    [CreateAssetMenu(fileName = "NewWaveConfig", menuName = "NeonDefense/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        public List<EnemyGroup> enemyGroups;
        public float timeBetweenGroups = 2f;
    }
}