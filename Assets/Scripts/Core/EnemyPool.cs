using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.Core
{
    public class EnemyPool : ObjectPool<Enemy>
    {
        public static EnemyPool Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            base.Awake();
        }
    }
}
