using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    public class EnemyPool : ObjectPool<Enemy>
    {
        public static EnemyPool Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            base.Awake();
        }
    }
}
