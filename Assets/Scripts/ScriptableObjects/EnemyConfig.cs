using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Enemy Stats")]
    public string enemyName = "Virus";
    [Tooltip("Health points of the enemy")]
    public float health = 100f;
    [Tooltip("Movement speed")]
    public float speed = 5f;
    [Tooltip("Amount of bits dropped on death")]
    public int bitDrop = 10;

    [Header("Visuals")]
    public GameObject prefab;
}
