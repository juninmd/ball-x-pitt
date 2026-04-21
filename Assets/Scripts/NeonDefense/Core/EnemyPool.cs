using UnityEngine;

namespace NeonDefense.Core
{
    [DisallowMultipleComponent]
    public class EnemyPool : ObjectPool<Enemies.Enemy>
    {
        public static EnemyPool Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}