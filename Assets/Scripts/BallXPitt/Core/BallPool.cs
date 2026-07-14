using System.Collections.Generic;
using UnityEngine;

namespace BallXPitt.Core
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        // Dictionary to support multiple ball types using their BallConfig InstanceID as key
        private Dictionary<int, Queue<Ball>> pools = new Dictionary<int, Queue<Ball>>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PreAllocate(ScriptableObjects.BallConfig config, int count)
        {
            if (config.prefab == null || !config.prefab.TryGetComponent<Ball>(out var ballPrefab)) return;

            int key = config.GetInstanceID();

            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            for (int i = 0; i < count; i++)
            {
                Ball newObj = Instantiate(ballPrefab, transform);
                newObj.gameObject.SetActive(false);
                pools[key].Enqueue(newObj);
            }
        }

        public Ball Get(ScriptableObjects.BallConfig config, Vector3 position, Quaternion rotation)
        {
            if (config.prefab == null || !config.prefab.TryGetComponent<Ball>(out var ballPrefab)) return null;

            int key = config.GetInstanceID();

            if (pools.ContainsKey(key) && pools[key].Count > 0)
            {
                Ball obj = pools[key].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
                return obj;
            }

            // Fallback if pool is empty (or not pre-allocated)
            Ball newObj = Instantiate(ballPrefab, position, rotation, transform);
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        public void ReturnToPool(Ball obj, ScriptableObjects.BallConfig config)
        {
            int key = config.GetInstanceID();

            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            obj.gameObject.SetActive(false);
            pools[key].Enqueue(obj);
        }
    }
}