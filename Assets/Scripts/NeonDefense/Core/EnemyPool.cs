using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.Core
{
    [DisallowMultipleComponent]
    public class EnemyPool : ObjectPool<Enemy>
    {
        public static EnemyPool Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
