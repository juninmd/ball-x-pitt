using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        private Dictionary<int, Queue<T>> poolDictionary = new Dictionary<int, Queue<T>>();

        public void PreAllocate(T prefab, int count)
        {
            int instanceId = prefab.GetInstanceID();
            if (!poolDictionary.ContainsKey(instanceId))
            {
                poolDictionary[instanceId] = new Queue<T>();
            }

            for (int i = 0; i < count; i++)
            {
                T newObj = Instantiate(prefab, transform);
                newObj.gameObject.SetActive(false);
                poolDictionary[instanceId].Enqueue(newObj);
            }
        }

        public T Get(T prefab)
        {
            int instanceId = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(instanceId) && poolDictionary[instanceId].Count > 0)
            {
                T obj = poolDictionary[instanceId].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            // Fallback (should ideally be avoided via PreAllocate)
            T newObj = Instantiate(prefab, transform);
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        public void ReturnToPool(T obj, T prefab)
        {
            obj.gameObject.SetActive(false);
            int instanceId = prefab.GetInstanceID();

            if (!poolDictionary.ContainsKey(instanceId))
            {
                poolDictionary[instanceId] = new Queue<T>();
            }

            poolDictionary[instanceId].Enqueue(obj);
        }
    }
}
