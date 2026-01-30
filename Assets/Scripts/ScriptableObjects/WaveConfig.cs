using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWaveConfig", menuName = "NeonDefense/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [System.Serializable]
    public struct EnemyGroup
    {
        public EnemyConfig enemyConfig;
        public int count;
        public float spawnRate;
    }

    public List<EnemyGroup> enemyGroups;
    public float timeBetweenGroups = 2f;
}
