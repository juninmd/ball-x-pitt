using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        private Dictionary<int, Queue<T>> poolDictionary = new Dictionary<int, Queue<T>>();
        // Instances currently queued, so a double ReturnToPool cannot hand the same object out twice.
        private readonly HashSet<T> queued = new HashSet<T>();

        public void PreAllocate(T prefab, int count)
        {
            int key = prefab.GetInstanceID();
            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<T>();
            }

            for (int i = 0; i < count; i++)
            {
                T newObj = Instantiate(prefab, transform);
                newObj.gameObject.SetActive(false);
                poolDictionary[key].Enqueue(newObj);
                queued.Add(newObj);
            }
        }

        public T Get(T prefab)
        {
            int key = prefab.GetInstanceID();
            if (poolDictionary.TryGetValue(key, out Queue<T> queue))
            {
                while (queue.Count > 0)
                {
                    T obj = queue.Dequeue();
                    queued.Remove(obj);
                    // Skip instances destroyed while they sat in the pool (e.g. on scene unload)
                    if (obj == null) continue;
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            // Fallback if not pre-allocated enough
            T fallbackObj = Instantiate(prefab, transform);
            fallbackObj.gameObject.SetActive(true);
            return fallbackObj;
        }

        public void ReturnToPool(T obj, T prefab)
        {
            if (obj == null || !queued.Add(obj)) return;
            obj.gameObject.SetActive(false);
            int key = prefab.GetInstanceID();
            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<T>();
            }
            poolDictionary[key].Enqueue(obj);
        }
    }
}