using System.Collections.Generic;
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> poolDictionary = new Dictionary<int, Queue<Ball>>();
        private Transform poolParent;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            poolParent = new GameObject("BallsPool").transform;
            poolParent.SetParent(transform);
        }

        public void PreAllocate(BallConfig config, int amount)
        {
            if (config == null || config.prefab == null) return;

            int key = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<Ball>();
            }

            for (int i = 0; i < amount; i++)
            {
                Ball newBall = Instantiate(config.prefab, poolParent);
                newBall.gameObject.SetActive(false);
                poolDictionary[key].Enqueue(newBall);
            }
        }

        public Ball Get(BallConfig config, Vector3 position, Quaternion rotation)
        {
            if (config == null || config.prefab == null) return null;

            int key = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<Ball>();
            }

            Ball ballToSpawn;

            if (poolDictionary[key].Count > 0)
            {
                ballToSpawn = poolDictionary[key].Dequeue();
                ballToSpawn.transform.position = position;
                ballToSpawn.transform.rotation = rotation;
            }
            else
            {
                // Fallback Se o pool acabar (idealmente PreAllocate deve ser suficiente)
                ballToSpawn = Instantiate(config.prefab, position, rotation, poolParent);
            }

            ballToSpawn.gameObject.SetActive(true);
            return ballToSpawn;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            ball.gameObject.SetActive(false);

            int key = config.GetInstanceID();
            if (poolDictionary.ContainsKey(key))
            {
                poolDictionary[key].Enqueue(ball);
            }
        }
    }
}
