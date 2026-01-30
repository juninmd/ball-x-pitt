using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct EnemyGroup
{
    public EnemyConfig enemyConfig;
    public int count;
    public float spawnRate;
}

[CreateAssetMenu(fileName = "NewWaveConfig", menuName = "NeonDefense/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [Header("Wave Settings")]
    public List<EnemyGroup> enemyGroups;
    public float timeBetweenGroups = 2f;
}
