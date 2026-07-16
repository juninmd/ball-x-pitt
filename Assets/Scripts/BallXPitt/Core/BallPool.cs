using System.Collections.Generic;
using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> poolDictionary = new Dictionary<int, Queue<Ball>>();

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

        public void PreAllocate(BallConfig config, int count)
        {
            if (config == null || config.prefab == null) return;

            int key = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<Ball>());
            }

            for (int i = 0; i < count; i++)
            {
                Ball newBall = Instantiate(config.prefab, transform);
                newBall.gameObject.SetActive(false);
                poolDictionary[key].Enqueue(newBall);
            }
        }

        public Ball GetBall(BallConfig config, Vector3 position, Quaternion rotation)
        {
            int key = config.GetInstanceID();

            if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
            {
                Ball ball = poolDictionary[key].Dequeue();
                ball.transform.position = position;
                ball.transform.rotation = rotation;
                ball.gameObject.SetActive(true);
                return ball;
            }

            Ball fallbackBall = Instantiate(config.prefab, position, rotation, transform);
            fallbackBall.gameObject.SetActive(true);
            return fallbackBall;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (!ball.gameObject.activeInHierarchy) return;

            ball.gameObject.SetActive(false);
            int key = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<Ball>());
            }

            poolDictionary[key].Enqueue(ball);
        }
    }
}
