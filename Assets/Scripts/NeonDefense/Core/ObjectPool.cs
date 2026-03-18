using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Core
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        private Dictionary<int, Queue<T>> _poolDictionary = new Dictionary<int, Queue<T>>();

        public T Get(T prefab, Vector3 position = default, Quaternion rotation = default)
        {
            int instanceID = prefab.GetInstanceID();

            if (!_poolDictionary.ContainsKey(instanceID))
            {
                _poolDictionary[instanceID] = new Queue<T>();
            }

            Queue<T> poolQueue = _poolDictionary[instanceID];

            T obj;
            if (poolQueue.Count > 0)
            {
                obj = poolQueue.Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate(prefab, position, rotation);
            }

            return obj;
        }

        public void Return(T obj, T prefab)
        {
            int instanceID = prefab.GetInstanceID();

            if (!_poolDictionary.ContainsKey(instanceID))
            {
                _poolDictionary[instanceID] = new Queue<T>();
            }

            obj.gameObject.SetActive(false);
            _poolDictionary[instanceID].Enqueue(obj);
        }
    }
}
