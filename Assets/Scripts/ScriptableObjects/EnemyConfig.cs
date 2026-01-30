using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Attributes")]
    public string enemyName = "Virus";
    public float health = 100f;
    public float speed = 5f;
    public int bitDrop = 10;

    [Header("Visuals")]
    public GameObject prefab;
}
