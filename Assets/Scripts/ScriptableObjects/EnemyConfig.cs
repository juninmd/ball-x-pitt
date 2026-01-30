using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Attributes")]
    public string enemyName;
    public float health;
    public float speed;
    public int bitDropAmount;

    [Header("Visuals")]
    public GameObject prefab;
}
