using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    /// <summary>
    /// A generic object pooling system.
    /// </summary>
    /// <typeparam name="T">The component type to pool.</typeparam>
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected T prefab;
        [SerializeField] protected int initialPoolSize = 10;

        private Queue<T> pool = new Queue<T>();

        protected virtual void Awake()
        {
            if (prefab == null)
            {
                Debug.LogError($"Prefab not assigned in pool: {gameObject.name}");
                return;
            }

            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewPoolObject();
            }
        }

        private T CreateNewPoolObject()
        {
            T newObj = Instantiate(prefab, transform);
            newObj.gameObject.SetActive(false);
            pool.Enqueue(newObj);
            return newObj;
        }

        /// <summary>
        /// Retrieves an object from the pool. Creates a new one if pool is empty.
        /// </summary>
        public T Get()
        {
            if (pool.Count == 0)
            {
                CreateNewPoolObject();
            }

            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}
