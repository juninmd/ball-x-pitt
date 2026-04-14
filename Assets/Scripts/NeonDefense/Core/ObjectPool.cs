using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        private Dictionary<int, Queue<T>> pools = new Dictionary<int, Queue<T>>();

        public void PreAllocate(T prefab, int initialPoolSize)
        {
            int key = prefab.GetInstanceID();
            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<T>();
            }

            for (int i = 0; i < initialPoolSize; i++)
            {
                T newObj = Instantiate(prefab);
                newObj.gameObject.SetActive(false);
                pools[key].Enqueue(newObj);
            }
        }

        public T Get(T prefab)
        {
            int key = prefab.GetInstanceID();

            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<T>();
            }

            if (pools[key].Count > 0)
            {
                T obj = pools[key].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            T newObj = Instantiate(prefab);
            return newObj;
        }

        public void ReturnToPool(T obj, T prefab)
        {
            obj.gameObject.SetActive(false);
            int key = prefab.GetInstanceID();

            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<T>();
            }

            pools[key].Enqueue(obj);
        }
    }
}
