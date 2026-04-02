using UnityEngine;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Enemies
{
    // Dummy implementation to allow other scripts to compile
    public class Enemy : MonoBehaviour
    {
        public EnemyConfig config;

        public void DestroyEnemy()
        {
            if (NeonDefense.Core.EnemyPool.Instance != null && config != null && config.prefab != null)
            {
                NeonDefense.Core.EnemyPool.Instance.ReturnToPool(this, config.prefab);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
