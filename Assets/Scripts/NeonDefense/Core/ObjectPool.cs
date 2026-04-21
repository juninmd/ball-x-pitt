using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        private Dictionary<int, Queue<T>> poolDictionary = new Dictionary<int, Queue<T>>();

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
            }
        }

        public T Get(T prefab)
        {
            int key = prefab.GetInstanceID();
            if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
            {
                T obj = poolDictionary[key].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            // Fallback if not pre-allocated enough
            T fallbackObj = Instantiate(prefab, transform);
            fallbackObj.gameObject.SetActive(true);
            return fallbackObj;
        }

        public void ReturnToPool(T obj, T prefab)
        {
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