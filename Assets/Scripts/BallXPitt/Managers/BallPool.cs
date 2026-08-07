using System.Collections.Generic;
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

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

        public void PreAllocate(BallConfig config, int amount)
        {
            int key = config.GetInstanceID();
            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            for (int i = 0; i < amount; i++)
            {
                Ball newBall = InstantiateBall(config);
                newBall.gameObject.SetActive(false);
                pools[key].Enqueue(newBall);
            }
        }

        public Ball GetBall(BallConfig config, Vector3 position, Quaternion rotation)
        {
            int key = config.GetInstanceID();

            if (pools.ContainsKey(key) && pools[key].Count > 0)
            {
                Ball ball = pools[key].Dequeue();
                ball.transform.position = position;
                ball.transform.rotation = rotation;
                ball.gameObject.SetActive(true);
                return ball;
            }

            // Fallback instantiation if pool is empty
            Ball newBall = InstantiateBall(config);
            newBall.transform.position = position;
            newBall.transform.rotation = rotation;
            newBall.gameObject.SetActive(true);
            return newBall;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            int key = config.GetInstanceID();
            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            ball.gameObject.SetActive(false);
            pools[key].Enqueue(ball);
        }

        private Ball InstantiateBall(BallConfig config)
        {
            GameObject obj = Instantiate(config.prefab);
            obj.transform.SetParent(transform);
            Ball ball = obj.GetComponent<Ball>();
            if (ball == null)
            {
                ball = obj.AddComponent<Ball>();
            }
            return ball;
        }
    }
}
