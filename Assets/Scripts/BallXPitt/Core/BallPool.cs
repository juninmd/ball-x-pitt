using System.Collections.Generic;
using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> poolDictionary;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                poolDictionary = new Dictionary<int, Queue<Ball>>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PreAllocate(BallConfig config, int count)
        {
            int poolKey = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary[poolKey] = new Queue<Ball>();
            }

            for (int i = 0; i < count; i++)
            {
                Ball newBall = CreateNewBall(config);
                newBall.gameObject.SetActive(false);
                poolDictionary[poolKey].Enqueue(newBall);
            }
        }

        public Ball Get(BallConfig config, Vector3 position, Quaternion rotation)
        {
            int poolKey = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary[poolKey] = new Queue<Ball>();
            }

            Ball ballToSpawn;

            if (poolDictionary[poolKey].Count > 0)
            {
                ballToSpawn = poolDictionary[poolKey].Dequeue();
            }
            else
            {
                // Fallback instantiation if pool is empty
                ballToSpawn = CreateNewBall(config);
            }

            ballToSpawn.transform.position = position;
            ballToSpawn.transform.rotation = rotation;
            ballToSpawn.gameObject.SetActive(true);
            ballToSpawn.Initialize(config);

            return ballToSpawn;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            int poolKey = config.GetInstanceID();
            ball.gameObject.SetActive(false);

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary[poolKey] = new Queue<Ball>();
            }

            poolDictionary[poolKey].Enqueue(ball);
        }

        private Ball CreateNewBall(BallConfig config)
        {
            GameObject obj = Instantiate(config.prefab);

            Ball ballComponent = obj.GetComponent<Ball>();
            if (ballComponent == null)
            {
                ballComponent = obj.AddComponent<Ball>();
            }

            obj.transform.SetParent(this.transform);
            return ballComponent;
        }
    }
}
