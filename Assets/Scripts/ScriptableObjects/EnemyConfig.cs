using UnityEngine;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Attributes")]
        public string enemyName;
        public float health;
        public float speed;
        public int bitDrop;
        public int damageToPlayer;

        [Header("Visuals")]
        public GameObject prefab;
    }
}
