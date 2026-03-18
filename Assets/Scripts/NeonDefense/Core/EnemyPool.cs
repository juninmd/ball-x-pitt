using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    [DisallowMultipleComponent]
    public class EnemyPool : ObjectPool<Enemies.Enemy>
    {
        private static EnemyPool _instance;

        public static EnemyPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EnemyPool>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("EnemyPool");
                        _instance = obj.AddComponent<EnemyPool>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
