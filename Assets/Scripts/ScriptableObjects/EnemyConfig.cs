using UnityEngine;

namespace NeonDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "NeonDefense/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("General")]
        public string enemyName;
        public GameObject prefab;

        [Header("Stats")]
        public float health;
        public float speed;
        public int bitDrop;
        public int damageToPlayer;
    }
}
