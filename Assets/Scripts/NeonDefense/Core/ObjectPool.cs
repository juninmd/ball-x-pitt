using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        private Dictionary<int, Queue<T>> poolDictionary = new Dictionary<int, Queue<T>>();

        public T Get(T prefab, Vector3 position = default, Quaternion rotation = default)
        {
            int key = prefab.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<T>());
            }

            if (poolDictionary[key].Count > 0)
            {
                T obj = poolDictionary[key].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
                return obj;
            }

            T newObj = Instantiate(prefab, position, rotation);
            return newObj;
        }

        public void ReturnToPool(T obj, T prefabType)
        {
            obj.gameObject.SetActive(false);
            int key = prefabType.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<T>());
            }

            poolDictionary[key].Enqueue(obj);
        }
    }
}
