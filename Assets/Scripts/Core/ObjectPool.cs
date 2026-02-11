using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    /// <summary>
    /// A generic object pooling system supporting multiple prefab variants.
    /// </summary>
    /// <typeparam name="T">The component type to pool.</typeparam>
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        [Tooltip("Default prefab to use if none specified.")]
        [SerializeField] protected T prefab;
        [SerializeField] protected int initialPoolSize = 10;

        // Key: Prefab Instance ID, Value: Queue of available objects
        private Dictionary<int, Queue<T>> pools = new Dictionary<int, Queue<T>>();

        // Key: Active Object Instance ID, Value: Prefab Instance ID (Origin)
        private Dictionary<int, int> activeObjectOrigins = new Dictionary<int, int>();

        protected virtual void Awake()
        {
            if (prefab != null)
            {
                InitializePool(prefab, initialPoolSize);
            }
        }

        private void InitializePool(T prefabToPool, int count)
        {
            int prefabId = prefabToPool.GetInstanceID();

            if (!pools.ContainsKey(prefabId))
            {
                pools[prefabId] = new Queue<T>();
            }

            for (int i = 0; i < count; i++)
            {
                CreateNewPoolObject(prefabToPool, prefabId);
            }
        }

        private T CreateNewPoolObject(T prefabToPool, int prefabId)
        {
            T newObj = Instantiate(prefabToPool, transform);
            newObj.gameObject.SetActive(false);

            // Register to the correct pool queue
            pools[prefabId].Enqueue(newObj);

            return newObj;
        }

        /// <summary>
        /// Retrieves an object from the pool using the default prefab.
        /// </summary>
        public T Get()
        {
            if (prefab == null)
            {
                Debug.LogError($"Default prefab not assigned in pool: {gameObject.name}");
                return null;
            }
            return Get(prefab);
        }

        /// <summary>
        /// Retrieves an object from the pool corresponding to the specified prefab.
        /// </summary>
        public T Get(T specificPrefab)
        {
            if (specificPrefab == null)
            {
                Debug.LogError("Cannot get null prefab from pool.");
                return null;
            }

            int prefabId = specificPrefab.GetInstanceID();

            // Ensure pool exists
            if (!pools.ContainsKey(prefabId))
            {
                pools[prefabId] = new Queue<T>();
            }

            Queue<T> queue = pools[prefabId];
            T obj;

            if (queue.Count == 0)
            {
                obj = CreateNewPoolObject(specificPrefab, prefabId);
                // CreateNewPoolObject enqueues it, so we dequeue immediately
                obj = queue.Dequeue();
            }
            else
            {
                obj = queue.Dequeue();
            }

            obj.gameObject.SetActive(true);
            activeObjectOrigins[obj.GetInstanceID()] = prefabId;

            return obj;
        }

        /// <summary>
        /// Returns an object to its original pool.
        /// </summary>
        public void ReturnToPool(T obj)
        {
            if (obj == null) return;

            int objId = obj.GetInstanceID();

            if (activeObjectOrigins.TryGetValue(objId, out int prefabId))
            {
                if (pools.ContainsKey(prefabId))
                {
                    obj.gameObject.SetActive(false);
                    pools[prefabId].Enqueue(obj);
                    activeObjectOrigins.Remove(objId);
                }
                else
                {
                    Debug.LogWarning($"Pool for object {obj.name} not found. Destroying.");
                    Destroy(obj.gameObject);
                }
            }
            else
            {
                Debug.LogWarning($"Object {obj.name} does not belong to this pool. Destroying.");
                Destroy(obj.gameObject);
            }
        }
    }
}
